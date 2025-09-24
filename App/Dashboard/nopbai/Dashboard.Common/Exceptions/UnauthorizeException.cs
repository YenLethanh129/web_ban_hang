namespace Dashboard.Common.Exceptions;

public class UnauthorizeException : Exception
{
    public UnauthorizeException(string message) : base(message)
    {
    }

    public UnauthorizeException(string message, Exception innerException) : base(message, innerException)
    {

    }
}