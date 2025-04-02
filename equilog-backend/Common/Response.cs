using equilog_backend.Common.Enums;

namespace equilog_backend.Common;

public class Response<T>
{
    public OperationResult OperationResult { get; }
    public T? Value { get; }
    public string? Error { get; }

    private Response(OperationResult operationResult, T value, string? error)
    {
        OperationResult = operationResult;
        Value = value;
        Error = error;
    }
    
    public static Response<T?> Success(T? value) => new(OperationResult.Success, value, null);
    
    public static Response<T?> Failure(string error) => new(OperationResult.GeneralError, default, error);

    public static Response<T?> ValidationError(string error) => new(OperationResult.ValidationError, default, error);
    
    public static Response<T?> NotFound(string error) => new(OperationResult.NotFound, default, error);
}