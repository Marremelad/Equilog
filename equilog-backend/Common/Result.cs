using System.Net;
using equilog_backend.Interfaces;

namespace equilog_backend.Common;

public static class Result
{
    public static IResult Generate(IApiResponse apiResponse)
    {
        return apiResponse.StatusCode switch
        {
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.BadRequest => Results.BadRequest(apiResponse),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            HttpStatusCode.Unauthorized => Results.Unauthorized(),
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.Conflict => Results.Conflict(apiResponse),
            _ => Results.Problem(apiResponse.Message)
        };
    }
}