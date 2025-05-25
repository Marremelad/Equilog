using System.Net;
using equilog_backend.Interfaces;

namespace equilog_backend.Common;

// Static utility class that converts ApiResponse objects into appropriate ASP.NET Core IResult responses.
// Maps HTTP status codes to their corresponding Result types for consistent API responses.
public static class Result
{
    public static IResult Generate(IApiResponse apiResponse)
    {
        return apiResponse.StatusCode switch
        {
            // 2xx Success.
            HttpStatusCode.OK => Results.Ok(apiResponse),
            HttpStatusCode.Created => Results.Json(apiResponse, statusCode: 201),
            HttpStatusCode.Accepted => Results.Json(apiResponse, statusCode: 202),
            HttpStatusCode.NonAuthoritativeInformation => Results.Json(apiResponse, statusCode: 203),
            HttpStatusCode.NoContent => Results.NoContent(),
            HttpStatusCode.ResetContent => Results.Json(apiResponse, statusCode: 205),
            HttpStatusCode.PartialContent => Results.Json(apiResponse, statusCode: 206),
            
            // 3xx Redirection.
            HttpStatusCode.MultipleChoices => Results.Json(apiResponse, statusCode: 300),
            HttpStatusCode.MovedPermanently => Results.Json(apiResponse, statusCode: 301),
            HttpStatusCode.Found => Results.Json(apiResponse, statusCode: 302),
            HttpStatusCode.SeeOther => Results.Json(apiResponse, statusCode: 303),
            HttpStatusCode.NotModified => Results.Json(apiResponse, statusCode: 304),
            HttpStatusCode.UseProxy => Results.Json(apiResponse, statusCode: 305),
            HttpStatusCode.TemporaryRedirect => Results.Json(apiResponse, statusCode: 307),
            HttpStatusCode.PermanentRedirect => Results.Json(apiResponse, statusCode: 308),
            
            // 4xx Client Errors.
            HttpStatusCode.BadRequest => Results.BadRequest(apiResponse),
            HttpStatusCode.Unauthorized => Results.Unauthorized(),
            HttpStatusCode.PaymentRequired => Results.Json(apiResponse, statusCode: 402),
            HttpStatusCode.Forbidden => Results.Forbid(),
            HttpStatusCode.NotFound => Results.NotFound(apiResponse),
            HttpStatusCode.MethodNotAllowed => Results.Json(apiResponse, statusCode: 405),
            HttpStatusCode.NotAcceptable => Results.Json(apiResponse, statusCode: 406),
            HttpStatusCode.ProxyAuthenticationRequired => Results.Json(apiResponse, statusCode: 407),
            HttpStatusCode.RequestTimeout => Results.Json(apiResponse, statusCode: 408),
            HttpStatusCode.Conflict => Results.Conflict(apiResponse),
            HttpStatusCode.Gone => Results.Json(apiResponse, statusCode: 410),
            HttpStatusCode.LengthRequired => Results.Json(apiResponse, statusCode: 411),
            HttpStatusCode.PreconditionFailed => Results.Json(apiResponse, statusCode: 412),
            HttpStatusCode.RequestEntityTooLarge => Results.Json(apiResponse, statusCode: 413),
            HttpStatusCode.RequestUriTooLong => Results.Json(apiResponse, statusCode: 414),
            HttpStatusCode.UnsupportedMediaType => Results.Json(apiResponse, statusCode: 415),
            HttpStatusCode.RequestedRangeNotSatisfiable => Results.Json(apiResponse, statusCode: 416),
            HttpStatusCode.ExpectationFailed => Results.Json(apiResponse, statusCode: 417),
            HttpStatusCode.MisdirectedRequest => Results.Json(apiResponse, statusCode: 421),
            HttpStatusCode.UnprocessableEntity => Results.UnprocessableEntity(apiResponse),
            HttpStatusCode.Locked => Results.Json(apiResponse, statusCode: 423),
            HttpStatusCode.FailedDependency => Results.Json(apiResponse, statusCode: 424),
            HttpStatusCode.UpgradeRequired => Results.Json(apiResponse, statusCode: 426),
            HttpStatusCode.PreconditionRequired => Results.Json(apiResponse, statusCode: 428),
            HttpStatusCode.TooManyRequests => Results.Json(apiResponse, statusCode: 429),
            HttpStatusCode.RequestHeaderFieldsTooLarge => Results.Json(apiResponse, statusCode: 431),
            HttpStatusCode.UnavailableForLegalReasons => Results.Json(apiResponse, statusCode: 451),
            
            // 5xx Server Errors.
            HttpStatusCode.InternalServerError => Results.Problem(apiResponse.Message, statusCode: 500),
            HttpStatusCode.NotImplemented => Results.Json(apiResponse, statusCode: 501),
            HttpStatusCode.BadGateway => Results.Json(apiResponse, statusCode: 502),
            HttpStatusCode.ServiceUnavailable => Results.Json(apiResponse, statusCode: 503),
            HttpStatusCode.GatewayTimeout => Results.Json(apiResponse, statusCode: 504),
            HttpStatusCode.HttpVersionNotSupported => Results.Json(apiResponse, statusCode: 505),
            HttpStatusCode.VariantAlsoNegotiates => Results.Json(apiResponse, statusCode: 506),
            HttpStatusCode.InsufficientStorage => Results.Json(apiResponse, statusCode: 507),
            HttpStatusCode.LoopDetected => Results.Json(apiResponse, statusCode: 508),
            HttpStatusCode.NotExtended => Results.Json(apiResponse, statusCode: 510),
            HttpStatusCode.NetworkAuthenticationRequired => Results.Json(apiResponse, statusCode: 511),
            
            // Default fallback for any unhandled status codes.
            _ => Results.Problem(apiResponse.Message)
        };
    }
}