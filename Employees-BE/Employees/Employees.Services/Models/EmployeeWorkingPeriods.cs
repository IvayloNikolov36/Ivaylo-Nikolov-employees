namespace Employees.Services.Models;

public class EmployeeWorkingPeriods
{
    public int EmployeeId { get; set; }

    public Period[] WorkingPeriods { get; set; } = [];
}
