using FluentValidation;

namespace equilog_backend.Common;

// Generic endpoint filter that validates request DTOs using FluentValidation
// Applied to API endpoints to automatically validate incoming request data.
public class ValidationFilter<T> : IEndpointFilter
{
    // Main filter method that executes before the endpoint handler.
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        // Get the appropriate validator for type T from dependency injection.
        var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();

        // Find the first argument of type T from the endpoint method parameters.
        var modelToValidate = context.Arguments.OfType<T>().FirstOrDefault();

        // Only validate if we found a model of the expected type.
        if (modelToValidate != null)
        {
            // Run FluentValidation rules against the model.
            var validationResult = await validator.ValidateAsync(modelToValidate);
            
            // If validation fails, return a validation problem response immediately.
            if (!validationResult.IsValid)
            {
                // Convert validation errors to ASP.NET Core's validation problem format.
                return Results.ValidationProblem(validationResult.ToDictionary());
            }
        }

        // Validation passed or no model found - continue to the actual endpoint.
        return await next(context);
    }
}