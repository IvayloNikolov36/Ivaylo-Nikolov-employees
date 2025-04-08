using CsvHelper.Configuration;
using Employees.Services.CSVHelper.Models;
using static Employees.Services.CSVHelper.Constants.CSVConstants;

namespace Employees.Services.CSVHelper.Mappers;

public sealed class EmployeeMapper : ClassMap<EmployeeModel>
{
    public EmployeeMapper()
    {
        this.Map(m => m.EmployeeId).Name(EmployeeId);
        this.Map(m => m.ProjectId).Name(ProjectId);
        this.Map(m => m.DateFrom).Name(DateFrom);
        this.Map(m => m.DateTo).Name(DateTo);
    }
}
