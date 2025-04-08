namespace Employees.ViewModels;

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }

    public int ProjectId { get; set; }

    public DateTime DateFrom { get; set; }

    public DateTime? DateTo { get; set; }
}
