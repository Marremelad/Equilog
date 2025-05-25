using equilog_backend.Interfaces;
using System.Net;
using System.Text.Json.Serialization;
namespace equilog_backend.Common;

// Generic wrapper class for standardizing API responses across the application.
public class ApiResponse<T> : IApiResponse
{
    // Indicates whether the operation was successful.
    public bool IsSuccess { get; init; }
    
    // HTTP status code for the response.
    public HttpStatusCode StatusCode { get; set; }
    
    // The actual data payload of the response (can be null for failures).
    public T? Value { get; init; }
    
    // Optional message providing additional context about the operation.
    public string? Message { get; set; }

    // Private constructor ensures responses are created only through factory methods.
    [JsonConstructor]
    private ApiResponse(bool isSuccess, HttpStatusCode statusCode, T value, string? message)
    {
        IsSuccess = isSuccess;
        StatusCode = statusCode;
        Value = value;
        Message = message;
    }

    // Factory method for creating successful API responses.
    public static ApiResponse<T?> Success(HttpStatusCode statusCode, T? value, string? message)
    {
        return new ApiResponse<T?>(true, statusCode, value, message);
    }

    // Factory method for creating failed API responses with no data payload.
    public static ApiResponse<T?> Failure(HttpStatusCode statusCode, string? message)
    {
        return new ApiResponse<T?>(false, statusCode, default, message);
    }
}