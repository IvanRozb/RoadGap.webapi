using Microsoft.AspNetCore.Mvc;

namespace RoadGap.webapi.Helpers;

public class RepositoryResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; } = "";
    public int StatusCode { get; set; }

    public static RepositoryResponse<T> CreateSuccess(T data, string message = "")
    {
        return new RepositoryResponse<T>
        {
            Data = data,
            Success = true,
            Message = message,
            StatusCode = 200
        };
    }

    public static RepositoryResponse<T> CreateNotFound(string message = "")
    {
        return new RepositoryResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = 404
        };
    }

    public static RepositoryResponse<T> CreateBadRequest(string message = "")
    {
        return new RepositoryResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = 400
        };
    }

    public static RepositoryResponse<T> CreateInternalServerError(string message = "")
    {
        return new RepositoryResponse<T>
        {
            Success = false,
            Message = message,
            StatusCode = 500
        };
    }

    public IActionResult ToActionResult()
    {
        if (Success)
        {
            return new OkObjectResult(new { message = Message, data = Data });
        }

        if (StatusCode == 404)
        {
            return new NotFoundObjectResult(new { message = Message });
        }

        return new BadRequestObjectResult(new { message = Message });
    }
}
