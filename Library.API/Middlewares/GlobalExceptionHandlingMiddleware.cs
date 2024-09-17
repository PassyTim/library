using System.Net;
using FluentValidation;
using Library.Application.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Library.API.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (AuthenticationException authException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "Authentication error",
                Title = "Authentication error",
                Detail = authException.Message
            };

            HandleException(context, problemDetails);
        }
        catch (TokenException tokenException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "Token error",
                Title = "Token error",
                Detail = tokenException.Message
            };

            HandleException(context, problemDetails);
        }
        catch (ValidationException validationException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.BadRequest,
                Type = "Validation error",
                Title = "Validation error",
                Detail = validationException.Message
            };

            HandleException(context, problemDetails);
        }
        catch (ItemNotFoundException notFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.NotFound,
                Type = "Request error",
                Title = "Item not found",
                Detail = notFoundException.Message
            };

            HandleException(context, problemDetails);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occured",
            };

            HandleException(context, problemDetails);
        }
    }

    private async void HandleException(HttpContext context, ProblemDetails problemDetails)
    {
        var jsonProblem = JsonConvert.SerializeObject(problemDetails);
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(jsonProblem);
    }
}