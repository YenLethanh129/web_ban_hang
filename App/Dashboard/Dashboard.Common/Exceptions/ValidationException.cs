namespace Dashboard.Common.Exceptions;

public class ValidationError
{
    public ValidationError(string propertyName, string errorMessage)
    {
        PropertyName = propertyName;
        ErrorMessage = errorMessage;
    }

    public string PropertyName { get; }
    public string ErrorMessage { get; }
}

public class ValidationException : Exception
{
    public ValidationException(List<ValidationError> errors) : base("One or more validation errors occurred.")
    {
        Errors = errors;
    }

    public List<ValidationError> Errors { get; }
}