using CsvHelper.Configuration.Attributes;
using Employees.Services.CSVHelper.Constants;

namespace Employees.Services.CSVHelper.Models;

public class EmployeeModel
{
    [Name(CSVConstants.EmployeeId)]
    public int EmployeeId { get; set; }

    [Name(CSVConstants.ProjectId)]
    public int ProjectId { get; set; }

    [Name(CSVConstants.DateFrom)]
    public DateTime DateFrom { get; set; }

    [Name(CSVConstants.DateTo)]
    public DateTime? DateTo { get; set; }
}
