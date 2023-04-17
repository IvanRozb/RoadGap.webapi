using RoadGap.webapi.Dtos;
using RoadGap.webapi.Models;
using RoadGap.webapi.Repositories;
using RoadGap.webapi.Repositories.Implementation;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

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
    config.CreateMap<CategoryToUpsertDto, Category>();
    config.CreateMap<TaskToUpsertDto, TaskModel>();
});
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

var app = builder.Build();

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
