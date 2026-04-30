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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Configurar la tabla base
            modelBuilder.Entity<Tarea>()
                .ToTable("Tareas") // Nombre de la tabla principal
                .Property(t => t.Titulo)
                .HasMaxLength(200)
                .IsRequired();

            // 2. Configurar las tablas hijas (Esto activa TPT automáticamente)
            // Al darles un nombre de tabla distinto, EF Core crea la herencia TPT
            modelBuilder.Entity<TareaSimple>().ToTable("TareasSimples");
            modelBuilder.Entity<TareaRecurrente>().ToTable("TareasRecurrentes");
            modelBuilder.Entity<TareaUrgente>().ToTable("TareasUrgentes");
            modelBuilder.Entity<TareaPomodoro>().ToTable("TareasPomodoro");
            modelBuilder.Entity<TareaConTarea>().ToTable("TareasConTarea");

            // 3. Configurar Usuario
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();
        }
    }
}

