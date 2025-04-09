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
    public IActionResult UploadEmployeesData([FromForm] IFormFile file, [FromForm] string dateFormat)
    {
        IEnumerable<ProjectEmployeesDataViewModel> employeesData = this.employeesImportService
            .GetEmployeesData(file.OpenReadStream(), dateFormat);

        return this.Ok(employeesData);
    }
}
