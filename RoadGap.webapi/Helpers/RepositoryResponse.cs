namespace RoadGap.webapi.Helpers;

public class RepositoryResponse<T>
{
    public T? Data { get; set; }
    public bool Success { get; set; }
    public string? Message { get; set; } = "";
    public int StatusCode { get; set; }
}
