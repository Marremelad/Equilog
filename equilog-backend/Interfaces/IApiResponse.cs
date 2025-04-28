using System.Net;

namespace equilog_backend.Interfaces;

public interface IApiResponse
{
    HttpStatusCode StatusCode { get; set; }
    string? Message { get; set; }
}