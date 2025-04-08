using Employees.Services.Contracts;
using Employees.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Web.Controllers;

public class EmployeesController : ApiController
{
    private readonly IEmployesImportService employeesImportService;

    public EmployeesController(IEmployesImportService employeesImportService)
    {
        this.employeesImportService = employeesImportService;
    }

    [HttpPost]
    [Route("upload")]
    public IActionResult UploadEmployeesData(IFormFile file)
    {
        IEnumerable<ProjectEmployeesDataViewModel> employeesData = this.employeesImportService
            .GetEmployeesData(file.OpenReadStream());

        return this.Ok(employeesData);
    }
}
