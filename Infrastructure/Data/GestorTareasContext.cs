using System;
using System.Collections.Generic;
using System.Text;
using GestorTarea.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GestorTarea.Infrastructure.Data
{
    public class GestorTareasContext : DbContext
    {

        public GestorTareasContext(DbContextOptions<GestorTareasContext> options) 
            : base(options)
        {
        }

        // Cada DbSet representa una tabla en la BD
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Tarea> Tareas { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // 1. Mapeo de Tablas TPT 
            modelBuilder.Entity<Tarea>().ToTable("Tareas");
            modelBuilder.Entity<TareaSimple>().ToTable("TareasSimples");
            modelBuilder.Entity<TareaRecurrente>().ToTable("TareasRecurrentes");
            modelBuilder.Entity<TareaPomodoro>().ToTable("TareasPomodoro");
            modelBuilder.Entity<TareaUrgente>().ToTable("TareasUrgentes");
            modelBuilder.Entity<TareaConTarea>().ToTable("TareasConTarea");

            // 2. Configuración de Tarea Base (Sincronizado con TablaTareas.sql)
            modelBuilder.Entity<Tarea>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID)
                      .HasColumnName("TareaID") // Nombre en el SQL
                      .ValueGeneratedOnAdd();   // IDENTITY(1,1)

                entity.Property(e => e.UsuarioID).HasColumnName("UsuarioID");
                entity.Property(e => e.Titulo).HasMaxLength(200).IsRequired();

                // Estado se mapea contra el campo privado _estado para que EF
                // lea/escriba el valor real y no pase por la lógica del getter
                // (que puede devolver Vencida y enmascarar Completada/Cancelada).
                entity.Property(e => e.Estado)
                      .HasField("_estado")
                      .UsePropertyAccessMode(PropertyAccessMode.Field);
            });

            // 3. Configuración de Usuario (Sincronizado con TablaUsuarios.sql)
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Usuario>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("UsuarioID"); // Coincide con SQL
                entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            });
        }
    }
}

