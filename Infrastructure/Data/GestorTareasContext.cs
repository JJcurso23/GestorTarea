using System;
using System.Collections.Generic;
using System.Text;
using GestorTarea.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorTarea.Infrastructure.Data
{
    public class GestorTareasContext : DbContext
    {
        // Cada DbSet representa una tabla en la BD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // Indicar a EF Core qué proveedor usar y cómo conectarse
            options.UseSqlServer(
                @"Server=localhost\MSSQLLocalDB;" +
                 "Database=GestorTareas;" +
                 "Trusted_Connection=True;" +
                 "TrustServerCertificate=True;"
            );
        }
    }
}

