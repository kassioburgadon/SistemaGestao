using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SistemaGestao.Data;
using System.Text.Json.Serialization;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<SistemaGestaoContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("SistemaGestaoContext") ?? throw new InvalidOperationException("Connection string 'SistemaGestaoContext' not found.")));

// Add services to the container.

builder.Services.AddControllers()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "SistemaGestao API v1");
    c.RoutePrefix = "swagger"; // acessível em /swagger
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
