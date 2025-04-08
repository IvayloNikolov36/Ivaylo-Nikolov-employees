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

    public IEnumerable<EmployeeViewModel> GetEmployeesData(Stream fileStream)
    {
        IEnumerable<EmployeeModel> data = this.csvService.ParseToEmployeeModel(fileStream);

        IEnumerable<EmployeeViewModel> employeesData = data
            .Select(em => new EmployeeViewModel
            {
                EmployeeId = em.EmployeeId,
                ProjectId = em.ProjectId,
                DateFrom = em.DateFrom,
                DateTo = em.DateTo
            });

        return employeesData;
    }
}
