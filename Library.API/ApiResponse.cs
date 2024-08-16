using System.Net;
using Library.Application.Contracts;

namespace Library.API;

public class ApiResponse
{
    public HttpStatusCode StatusCode { get; set; }
    
    public bool IsSuccess { get; set; } = true;
    public object? Data { get; set; } 
    public List<string> Errors { get; set; } = []; 
}