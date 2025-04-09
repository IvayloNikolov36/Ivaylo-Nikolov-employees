using System.Net.Mime;
using System.Net;
using System.Text.Json;
using Employees.Common.Exceptions;
using Employees.Web.Infrastructure.Models;

namespace Employees.Web.Infrastructure.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await this.next(context);
        }
        catch (Exception ex)
        {
            await this.HandleExceptionAsync(context, ex, Guid.NewGuid());
        }
    }

    private Task HandleExceptionAsync(HttpContext context, Exception exception, Guid guid)
    {
        context.Response.ContentType = MediaTypeNames.Application.Json;
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        HttpErrorResult errorResult = null;

        switch (exception)
        {
            case ActionableException actionableException:
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                errorResult = new HttpErrorResult(guid, actionableException);
                break;
            default:
                errorResult = new HttpErrorResult(guid);
                break;
        }

        JsonSerializerOptions options = new()
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        string jsonResult = JsonSerializer.Serialize(errorResult, options);

        return context.Response.WriteAsync(jsonResult);
    }
}
