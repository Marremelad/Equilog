using System.Net;

namespace equilog_backend.Interfaces;

public interface IApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    public string? Message { get; set; }
}