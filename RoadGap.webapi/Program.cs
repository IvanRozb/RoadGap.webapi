using LoggerService;
using NLog;
using RoadGap.webapi.Dtos;
using RoadGap.webapi.Extensions;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories;
using RoadGap.webapi.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Load configuration for logger manager
LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

// Add services to the container.
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("http://localhost:3000/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
    options.AddPolicy("ProdCors", policyBuilder =>
    {
        policyBuilder.WithOrigins("https://road-gap-front.vercel.app/")
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddAutoMapper(config =>
{
    config.CreateMap<UserToUpsertDto, User>();
    config.CreateMap<StatusToUpsertDto, Status>();
    config.CreateMap<CategoryToUpsertDto, Category>();
    config.CreateMap<TaskToUpsertDto, TaskModel>();
});

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();

var app = builder.Build();
app.ConfigureCustomExceptionMiddleware();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{   
    app.UseCors("ProdCors");
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
