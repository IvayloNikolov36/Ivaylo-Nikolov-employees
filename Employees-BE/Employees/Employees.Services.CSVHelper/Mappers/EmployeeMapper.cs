using CsvHelper.Configuration;
using Employees.Services.CSVHelper.Models;
using System.Globalization;
using static Employees.Services.CSVHelper.Constants.CSVConstants;

namespace Employees.Services.CSVHelper.Mappers;

public sealed class EmployeeMapper : ClassMap<EmployeeModel>
{
    public EmployeeMapper(string dateFormat)
    {
        this.ConfigureMappings(dateFormat);
    }

    private void ConfigureMappings(string dateFormat)
    {
        this.Map(m => m.EmployeeId).Name(EmployeeId);
        this.Map(m => m.ProjectId).Name(ProjectId);
        this.Map(m => m.DateFrom).Name(DateFrom).Convert(s =>
            DateTime.ParseExact(
                s.Row.Parser.Record![2].Trim(),
                dateFormat,
                CultureInfo.InvariantCulture));
        this.Map(m => m.DateTo).Name(DateTo).Convert(s =>
            s.Row.Parser.Record![3].Trim() == string.Empty
                ? null
                : DateTime.ParseExact(
                    s.Row.Parser.Record![3].Trim(),
                    dateFormat,
                    CultureInfo.InvariantCulture));
    }
}
