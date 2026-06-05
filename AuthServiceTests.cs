using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using GestorTarea.Domain.Entities;
using GestorTarea.Domain.Interfaces;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace GestorTarea.Tests
{
    public class AuthServiceTests
    {
        private Mock<IUsuarioRepositorio> _mockRepositorio;
        private Mock<IConfiguration> _mockConfig;
        private AuthService _authService;

        [SetUp]
        public void Inicializar()
        {
            _mockRepositorio = new Mock<IUsuarioRepositorio>();
            _mockConfig = new Mock<IConfiguration>();
            _authService = new AuthService(_mockRepositorio.Object, _mockConfig.Object);
        }

        [Test]
        public void Login_DevuelveNull_CuandoUsaurioNoExiste()
        {
            _mockRepositorio.Setup(r => r.ObtenerPorEmail(It.IsAny<string>()))
                .Returns((Usuario?)null);

            var resultado = _authService.Login(new LoginDTO
            {
                Email = "Noexiste@test.com",
                Password = "123456"
            });

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void Login_DevuelveNull_CuandoPasswordEsIncorrecta()
        {
            _mockRepositorio.Setup(r => r.ObtenerPorEmail("ana@test.com"))
                .Returns(new Usuario 
                //Tuve que hacer publico el constructor para poder hacer pruebas.
                { 
                    Email = "ana@test.com",
                    Nombre = "Ana Test",
                    Activo = true,
                    Edad = 23,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("OtraMas")
                });

            var resultado = _authService.Login(
                new LoginDTO { Email = "ana@test.com", Password = "Burbuja14" });
            Assert.That(resultado, Is.Null);
        }
    }
}
