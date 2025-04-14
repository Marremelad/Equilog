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
                HttpStatusCode.NotFound => Results.NotFound(apiResponse),
                HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
                HttpStatusCode.NoContent => Results.NoContent(),
                _ => Results.Problem(apiResponse.Message)
            };
        }
    }
}
