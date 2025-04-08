using Employees.ViewModels;

namespace Employees.Services.Contracts;

public interface IEmployesImportService
{
    IEnumerable<ProjectEmployeesDataViewModel> GetEmployeesData(Stream fileStream);
}
