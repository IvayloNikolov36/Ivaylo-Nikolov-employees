using CsvHelper;
using Employees.Services.CSVHelper.Models;
using System.Globalization;

namespace Employees.Services.CSVHelper;

public class CsvParserService : ICsvParserService
{
    public IEnumerable<EmployeeModel> ParseToEmployeeModel(Stream fileStream)
    {
        using StreamReader streamReader = new(fileStream);
        using CsvReader csvReader = new(streamReader, CultureInfo.InvariantCulture);
        List<EmployeeModel> records = [.. csvReader.GetRecords<EmployeeModel>()];

        return records;
    }
}
