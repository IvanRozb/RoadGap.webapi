namespace RoadGap.webapi.Helpers;

public class RepositoryResponse<T>
{
    public T? Data { get; set; } = default(T);
    public bool Success { get; set; }
    public string ErrorMessage { get; set; } = "";
    public int StatusCode { get; set; }
}
