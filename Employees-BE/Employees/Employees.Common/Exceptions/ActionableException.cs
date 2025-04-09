namespace Employees.Common.Exceptions;

public class ActionableException : Exception
{
    public ActionableException(string message, Exception? innerException = null)
        : base(message, innerException)
    {

    }
}
