using api.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigurarServicos();

var app = builder.Build();

app.UsarServicos().Run();