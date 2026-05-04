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
                @"Server=(localdb)\MSSQLLocalDB;" +
                 "Database=GestorTareas;" +
                 "Trusted_Connection=True;" +
                 "TrustServerCertificate=True;"
            );
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Mapeo de Tablas TPT (Table Per Type)[cite: 7, 20]
            modelBuilder.Entity<Tarea>().ToTable("Tareas");
            modelBuilder.Entity<TareaSimple>().ToTable("TareasSimples");
            modelBuilder.Entity<TareaRecurrente>().ToTable("TareasRecurrentes");
            modelBuilder.Entity<TareaPomodoro>().ToTable("TareasPomodoro");
            modelBuilder.Entity<TareaUrgente>().ToTable("TareasUrgentes");
            modelBuilder.Entity<TareaConTarea>().ToTable("TareasConTarea");

            // 2. Configuración de Tarea Base (Sincronizado con TablaTareas.sql)[cite: 21]
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                      .HasColumnName("TareaID") // Nombre en el SQL[cite: 21]
                      .ValueGeneratedOnAdd();   // IDENTITY(1,1)[cite: 21]

                entity.Property(e => e.UsuarioID).HasColumnName("UsuarioID");
                entity.Property(e => e.Titulo).HasMaxLength(200).IsRequired();
            });

            // 3. Configuración de Usuario (Sincronizado con TablaUsuarios.sql)[cite: 22]
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("UsuarioID"); // Coincide con SQL[cite: 22]
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            });
        }
    }
}

