using GestorTarea.Domain.Interfaces;
using GestorTarea.Infrastructure.Data;
using GestorTarea.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using GestorTarea.Application.Services;

var builder = WebApplication.CreateBuilder(args);
// PARTE 1: registrar servicios
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Registrar DbContext
builder.Services.AddDbContext<GestorTareasContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Registrar Inyección de Dependencias
builder.Services.AddScoped<ITareaRepositorio, TareaRepositorio>();
builder.Services.AddScoped<GestorTareasService>();

// --------

var app = builder.Build();
// PARTE 2: configurar el pipeline de peticiones
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run(); 