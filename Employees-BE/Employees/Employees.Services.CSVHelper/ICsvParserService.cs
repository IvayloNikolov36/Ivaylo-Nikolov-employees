using Employees.Services.CSVHelper.Models;

namespace Employees.Services.CSVHelper;

public interface ICsvParserService
{
    IEnumerable<EmployeeModel> ParseToEmployeeModel(Stream fileStream, string dateFormat);
}
