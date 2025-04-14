using System.Net;
using equilog_backend.Interfaces;

namespace equilog_backend.Common
{
    public class Result
    {
        public static IResult Generate(IApiResponse apiResponse)
        {
            return apiResponse.StatusCode switch
            {
                HttpStatusCode.OK => Results.Ok(apiResponse),
                HttpStatusCode.BadRequest => Results.BadRequest(apiResponse),
                HttpStatusCode.NotFound => Results.NotFound(apiResponse),
                HttpStatusCode.NoContent => Results.NoContent(),
                HttpStatusCode.Unauthorized => Results.Json(apiResponse, statusCode: 401),
                HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
                _ => Results.Problem(apiResponse.Message)
            };
        }
    }
}
