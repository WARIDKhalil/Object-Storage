using API.Services;
using Domain.Aggregates.Content;
using Domain.Contracts;
using Infrastructure.Database;
using Infrastructure.Repositories;
using Infrastructure.Settings;
using MinioMiddleware;
using MinioMiddleware.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// settings
builder.Services.Configure<MinIOSettings>(builder.Configuration.GetSection("minio"));
builder.Services.Configure<MongodbSettings>(builder.Configuration.GetSection("mongo"));
// injection
builder.Services.AddSingleton<MongoContext>();
builder.Services.AddSingleton<MinioMiddlewareService>();
builder.Services.AddSingleton<ContentService>();
builder.Services.AddSingleton<IRepository<Content>, Repository<Content>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
