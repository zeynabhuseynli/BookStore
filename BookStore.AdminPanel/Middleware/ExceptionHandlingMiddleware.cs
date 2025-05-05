using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using BookStore.Application.Exceptions;

namespace BookStore.AdminPanel.Middleware;
public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private async Task HandleExceptionAsync(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "application/json";
        var response = context.Response;
        var problemDetails = new ProblemDetails();

        switch (ex)
        {
            case ApplicationException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Application Error";
                break;

            case KeyNotFoundException:
            case NotFoundException:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Not Found";
                break;
            case ValidationException exc:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails = new ValidationProblemDetails(exc.Errors);
                problemDetails.Detail = ex.Message;
                problemDetails.Extensions.Add("invalidParams", exc.Errors);
                problemDetails.Title = "Validation Error";
                break;
            case UnAuthorizedException:
                response.StatusCode = (int)HttpStatusCode.Unauthorized;
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Unauthorized";
                break;
            case ArgumentException:
            case BadRequestException:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                problemDetails.Detail = ex.Message;
                problemDetails.Title = "Bad Request";
                break;

            default:
                response.StatusCode = (int)HttpStatusCode.InternalServerError;
                problemDetails.Detail = $"{ex.Message}";
                problemDetails.Title = "Server error";
                break;
        }

        var result = JsonSerializer.Serialize(problemDetails);
        await context.Response.WriteAsync(result);
    }
}
