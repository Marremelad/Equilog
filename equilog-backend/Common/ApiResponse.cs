using equilog_backend.Interfaces;
using System.Net;
namespace equilog_backend.Common;

public class ApiResponse<T> : IApiResponse
{
    public bool IsSuccess { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public T? Value { get; set; }
    public string? Message { get; set; }

    private ApiResponse(bool isSuccess, HttpStatusCode statusCode, T value, string? message)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Value = value;
        Message = message;
    }

    public static ApiResponse<T?> Success(HttpStatusCode statusCode, T? value, string? message)
    {
        return new ApiResponse<T?>(true, statusCode, value, message);
    }

    public static ApiResponse<T?> Failure(HttpStatusCode statusCode, string? message)
    {
        return new ApiResponse<T?>(false, statusCode, default, message);
    }
}