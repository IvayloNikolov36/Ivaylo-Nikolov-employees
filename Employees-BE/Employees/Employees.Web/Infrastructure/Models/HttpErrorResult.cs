using Employees.Common.Exceptions;

namespace Employees.Web.Infrastructure.Models;

public class HttpErrorResult
{
    private const string ActionableErrorMessage = "Your request could not be completed!";
    private const string UnknownErrorMessage = "An unexpected situation occurred! The dev team would be notified!";

    private HttpErrorResult(Guid guid, string message)
    {
        this.Guid = guid;
        this.Message = message;
        this.Errors = new List<string>();
    }

    public HttpErrorResult(Guid guid)
        : this(guid, UnknownErrorMessage)
    {

    }

    public HttpErrorResult(Guid guid, ActionableException exception)
        : this(guid, ActionableErrorMessage)
    {
        this.PopulateErrors(exception);
        this.Errors = this.Errors.Reverse().ToList();
    }

    public Guid Guid { get; private set; }

    public string Message { get; private set; }

    public ICollection<string> Errors { get; private set; }

    private void PopulateErrors<TException>(TException exception) where TException : Exception
    {
        if (exception.InnerException is TException)
        {
            this.PopulateErrors(exception.InnerException as TException);
        }

        this.Errors.Add(exception.Message);
    }
}
