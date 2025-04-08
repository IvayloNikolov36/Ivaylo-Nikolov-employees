using Employees.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Web.Controllers;

public class EmployeesController : ApiController
{

    public EmployeesController()
    {

    }

    [HttpPost]
    [Route("upload")]
    public async Task<IActionResult> UploadEmployeesData(IFormFile file)
    {
        return this.Ok(file.FileName);
    }
}
