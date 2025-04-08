using Employees.ViewModels;

namespace Employees.Services.Contracts;

public interface IEmployesImportService
{
    IEnumerable<EmployeeViewModel> GetEmployeesData(Stream fileStream);
}
