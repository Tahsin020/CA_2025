using Microsoft.AspNetCore.Diagnostics;
using System.ComponentModel.DataAnnotations;
using TS.Result;

namespace CA_2025.WebAPI;

public sealed class ExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        Result<string> errorResult;

        httpContext.Response.ContentType = "application/json";
        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        if (exception is ValidationException validationException)
        {
            httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

            errorResult = Result<string>.Failure(StatusCodes.Status403Forbidden, new List<string> { validationException.Message });

            await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken: cancellationToken);

            return true;
        }

        errorResult = Result<string>.Failure(exception.Message);

        await httpContext.Response.WriteAsJsonAsync(errorResult, cancellationToken: cancellationToken);

        return true;
    }
}
