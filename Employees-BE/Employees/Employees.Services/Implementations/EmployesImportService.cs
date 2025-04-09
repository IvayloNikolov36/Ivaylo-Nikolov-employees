using Employees.Services.Contracts;
using Employees.Services.CSVHelper;
using Employees.Services.CSVHelper.Models;
using Employees.ViewModels;

namespace Employees.Services.Implementations;

public class EmployesImportService : IEmployesImportService
{
    private readonly ICsvParserService csvService;

    public EmployesImportService(ICsvParserService csvService)
    {
        this.csvService = csvService;
    }

    public IEnumerable<ProjectEmployeesDataViewModel> GetEmployeesData(Stream fileStream, string dateFormat)
    {
        IEnumerable<EmployeeModel> data = this.csvService
            .ParseToEmployeeModel(fileStream, dateFormat);

        var groupedData = data
            .GroupBy(x => x.ProjectId)
            .Select(gr => new
            {
                ProjectId = gr.Key,
                Employees = gr.ToArray()
            });

        Dictionary<int, ProjectData[]> dataByProjectId = new(groupedData.Count());

        foreach (var group in groupedData)
        {
            int projectId = group.ProjectId;

            Queue<ProjectData> projectData = this.GetEmployeesPairWorkingDays(group.Employees);

            if (projectData.Count == 0)
            {
                continue;
            }

            int maxDays = projectData
                .MaxBy(x => x.WorkingDays)!.WorkingDays;

            if (maxDays == 0)
            {
                continue;
            }

            ProjectData[] pairs = projectData
                .Where(x => x.WorkingDays == maxDays)
                .ToArray();

            dataByProjectId.Add(projectId, pairs);
        }

        List<ProjectEmployeesDataViewModel> result = new();

        foreach (var pair in dataByProjectId)
        {
            ProjectData[] employeesOnSameProject = pair.Value;

            foreach (ProjectData employeesPair in employeesOnSameProject)
            {
                result.Add(new ProjectEmployeesDataViewModel
                {
                    ProjectId = pair.Key,
                    FirstEmployeeId = employeesPair.FirstEmployeeId,
                    SecondEmployeeId = employeesPair.SecondEmployeeId,
                    Days = employeesPair.WorkingDays
                });
            }
        }

        return result;
    }

    private Queue<ProjectData> GetEmployeesPairWorkingDays(EmployeeModel[] employees)
    {
        Queue<ProjectData> queue = new();

        int employeeIndex = 0;

        while (employeeIndex < employees.Length - 1)
        {
            EmployeeModel employee = employees[employeeIndex];

            for (int i = employeeIndex + 1; i < employees.Length; i++)
            {
                EmployeeModel currentEmployee = employees[i];

                ProjectData projectData = new()
                {
                    FirstEmployeeId = employee.EmployeeId,
                    SecondEmployeeId = currentEmployee.EmployeeId,
                    WorkingDays = this.GetDaysTogether(employee, currentEmployee)
                };

                queue.Enqueue(projectData);
            }

            employeeIndex++;
        }

        return queue;
    }

    private int GetDaysTogether(EmployeeModel employee, EmployeeModel secondEmployee)
    {
        DateTime employeeDateTo = employee.DateTo ?? DateTime.UtcNow;
        DateTime secondEmployeeDateTo = secondEmployee.DateTo ?? DateTime.UtcNow;

        bool noCross = secondEmployee.DateFrom >= employeeDateTo
            || employee.DateFrom >= secondEmployeeDateTo;

        if (noCross)
        {
            return 0;
        }

        DateTime latestStartDate = employee.DateFrom > secondEmployee.DateFrom
            ? employee.DateFrom
            : secondEmployee.DateFrom;

        DateTime earliestEndDate = employeeDateTo < secondEmployeeDateTo
            ? employeeDateTo
            : secondEmployeeDateTo;

        TimeSpan span = earliestEndDate - latestStartDate;

        return span.Days;
    }
}
