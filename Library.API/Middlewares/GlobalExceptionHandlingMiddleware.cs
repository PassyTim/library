using System.Net;
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
        catch (ItemNotFoundException notFoundException)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.NotFound,
                Type = "Item not found",
                Title = "Item not found",
                Detail = "The server cannot find the requested resource."
            };

            var jsonProblem = JsonConvert.SerializeObject(problemDetails);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonProblem);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            ProblemDetails problemDetails = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occured"
            };

            var jsonProblem = JsonConvert.SerializeObject(problemDetails);
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(jsonProblem);
        }
    }
}