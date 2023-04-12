using Microsoft.EntityFrameworkCore;

namespace RoadGap.webapi.Data;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;
    public DataContext(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    
}