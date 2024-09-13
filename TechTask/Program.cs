using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTask.Data;
using TechTask.EndPoints;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<TechTaskContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TechTaskContext") ?? throw new InvalidOperationException("Connection string 'TechTaskContext' not found.")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(
    options => options.AddPolicy("PermitirAcessos", builder =>
    {
        builder.AllowAnyHeader();
        builder.AllowAnyMethod();
        builder.AllowAnyOrigin();
    }
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseCors("PermitirAcessos");

app.UseAuthorization();

app.MapControllers();

app.MapUsuariosEndpoints();
app.MapTarefasEndpoints();

app.Run();
