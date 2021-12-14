using Microsoft.EntityFrameworkCore;
using minimal_api;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<TodoDb>(opt => opt.UseInMemoryDatabase("TodoList"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.UseSwagger();

app.MapGet("/", () => "Hello World!");

app.UseSwaggerUI();

app.Run();
