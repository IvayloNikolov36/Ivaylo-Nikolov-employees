using Employees.Services.Contracts;
using Employees.Services.CSVHelper;
using Employees.Services.CSVHelper.Models;
using Employees.Services.Models;
using Employees.ViewModels;

namespace Employees.Services.Implementations;

public class EmployesImportService : IEmployesImportService
{
    private readonly ICsvParserService csvService;

    public EmployesImportService(ICsvParserService csvService)
    {
        this.csvService = csvService;
    }

    public IEnumerable<ProjectEmployeesDataViewModel> GetEmployeesData(
        Stream fileStream,
        string dateFormat)
    {
        IEnumerable<EmployeeModel> parsedEmployeesData = this.csvService
            .ParseToEmployeeModel(fileStream, dateFormat);

        var employeesDataGroupedByProject = parsedEmployeesData
            .GroupBy(x => x.ProjectId)
            .Select(gr => new
            {
                ProjectId = gr.Key,
                Employees = gr.ToArray()
            });

        Dictionary<int, ProjectData[]> dataByProjectId = new(employeesDataGroupedByProject.Count());

        foreach (var group in employeesDataGroupedByProject)
        {
            EmployeeWorkingPeriods[] employeesData = this.AggregateEmployeesWorkingPeriods(group.Employees);

            Queue<ProjectData> projectData = this.GetEmployeesPairWorkingDays(employeesData);
            if (projectData.Count == 0)
            {
                continue;
            }

            int maxDays = projectData.MaxBy(x => x.WorkingDays)!.WorkingDays;
            if (maxDays == 0)
            {
                continue;
            }

            ProjectData[] pairs = [.. projectData.Where(x => x.WorkingDays == maxDays)];

            dataByProjectId.Add(group.ProjectId, pairs);
        }

        return this.GetResultData(dataByProjectId);
    }

    private Queue<ProjectData> GetEmployeesPairWorkingDays(EmployeeWorkingPeriods[] employees)
    {
        Queue<ProjectData> queue = new();

        int employeeIndex = 0;

        while (employeeIndex < employees.Length - 1)
        {
            EmployeeWorkingPeriods employee = employees[employeeIndex];

            for (int i = employeeIndex + 1; i < employees.Length; i++)
            {
                EmployeeWorkingPeriods currentEmployee = employees[i];

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

    private int GetDaysTogether(EmployeeWorkingPeriods employee, EmployeeWorkingPeriods secondEmployee)
    {
        int totalDaysTogether = 0;

        foreach (Period employeePeriod in employee.WorkingPeriods)
        {
            Period firstEmpPeriod = new()
            {
                DateFrom = employeePeriod.DateFrom,
                DateTo = employeePeriod.DateTo ?? DateTime.UtcNow
            };

            foreach (Period secondEmployeePeriod in secondEmployee.WorkingPeriods)
            {
                Period secondEmpPeriod = new()
                {
                    DateFrom = secondEmployeePeriod.DateFrom,
                    DateTo = secondEmployeePeriod.DateTo ?? DateTime.UtcNow
                };

                totalDaysTogether += this.CalculateDaysTogether(firstEmpPeriod, secondEmpPeriod);
            }
        }

        return totalDaysTogether;
    }

    private EmployeeWorkingPeriods[] AggregateEmployeesWorkingPeriods(EmployeeModel[] employees)
    {
        return employees
            .GroupBy(emp => emp.EmployeeId)
            .Select(gr => new EmployeeWorkingPeriods
            {
                EmployeeId = gr.Key,
                WorkingPeriods = gr.Select(x => new Period
                {
                    DateFrom = x.DateFrom,
                    DateTo = x.DateTo
                }).ToArray()
            }).ToArray();
    }

    private IEnumerable<ProjectEmployeesDataViewModel> GetResultData(
        Dictionary<int, ProjectData[]> dataByProjectId)
    {
        List<ProjectEmployeesDataViewModel> result = new();
        int id = 0;

        foreach (KeyValuePair<int, ProjectData[]> pair in dataByProjectId)
        {
            ProjectData[] employeesOnSameProject = pair.Value;

            foreach (ProjectData employeesPair in employeesOnSameProject)
            {
                result.Add(new ProjectEmployeesDataViewModel
                {
                    Id = ++id,
                    ProjectId = pair.Key,
                    FirstEmployeeId = employeesPair.FirstEmployeeId,
                    SecondEmployeeId = employeesPair.SecondEmployeeId,
                    Days = employeesPair.WorkingDays
                });
            }
        }

        return result
            .OrderByDescending(r => r.Days)
            .ThenBy(r => r.ProjectId);
    }

    private int CalculateDaysTogether(Period firstEmployeePeriod, Period secondEmployeePeriod)
    {
        bool noCross = secondEmployeePeriod.DateFrom >= firstEmployeePeriod.DateTo
            || firstEmployeePeriod.DateFrom >= secondEmployeePeriod.DateTo;

        if (noCross)
        {
            return 0;
        }

        DateTime latestStartDate = firstEmployeePeriod.DateFrom > secondEmployeePeriod.DateFrom
            ? firstEmployeePeriod.DateFrom
            : secondEmployeePeriod.DateFrom;

        DateTime? earliestEndDate = firstEmployeePeriod.DateTo < secondEmployeePeriod.DateTo
            ? firstEmployeePeriod.DateTo
            : secondEmployeePeriod.DateTo;

        TimeSpan span = earliestEndDate!.Value - latestStartDate;

        return span.Days;
    }
}
