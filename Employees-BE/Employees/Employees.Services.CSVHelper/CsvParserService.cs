using CsvHelper;
using Employees.Common.Exceptions;
using Employees.Services.CSVHelper.Mappers;
using Employees.Services.CSVHelper.Models;
using System.Globalization;

namespace Employees.Services.CSVHelper;

public class CsvParserService : ICsvParserService
{
    public IEnumerable<EmployeeModel> ParseToEmployeeModel(Stream fileStream, string dateFormat)
    {
        using StreamReader streamReader = new(fileStream);
        using CsvReader csvReader = new(streamReader, CultureInfo.CurrentCulture);

        IEnumerable<EmployeeModel> records = [];
        csvReader.Context.RegisterClassMap(new EmployeeMapper(dateFormat));

        try
        {
            records = csvReader.GetRecords<EmployeeModel>().ToList();
        }
        catch (Exception ex)
        {
            throw new ActionableException(
                "Cannot parse the file data. Please check the data and selected date format and try again."
                + ex.InnerException?.Message);
        }
        
        return records;
    }
}
