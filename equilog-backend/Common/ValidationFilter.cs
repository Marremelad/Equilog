using FluentValidation;

namespace equilog_backend.Common
{
    public class ValidationFilter<T> : IEndpointFilter
    {
        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            var validator = context.HttpContext.RequestServices.GetRequiredService<IValidator<T>>();
            var modelToValidate = context.Arguments.OfType<T>().FirstOrDefault();

            if (modelToValidate != null)
            {
                var validationResult = await validator.ValidateAsync(modelToValidate);
                if (!validationResult.IsValid)
                {
                    return Results.ValidationProblem(validationResult.ToDictionary());
                }
            }

            return await next(context);
        }
    }
}
