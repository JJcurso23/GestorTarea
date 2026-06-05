using GestorTarea.Application.DTOs;
using GestorTarea.Application.Services;
using GestorTarea.Domain.Entities;
using GestorTarea.Domain.Enums;
using GestorTarea.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace GestorTarea.Tests
{
    [TestFixture]
    public class GestorTareasServiceTests
    {
        private Mock<ITareaRepositorio> _mockRepositorio;
        private Mock<IUsuarioRepositorio> _mockUsuarios;
        private GestorTareasService _servicio;

        // Constantes para hacer los tests mas legibles
        private const int USUARIO_ID = 1;
        private const int OTRO_USUARIO_ID = 99;
        private const bool ES_ADMIN = true;
        private const bool NO_ES_ADMIN = false;

        // Helpers de fechas (siempre relativas a "ahora" para evitar tests fragiles)
        private static DateTime FechaFutura(int dias = 30) => DateTime.Now.AddDays(dias);
        private static DateTime FechaPasada(int dias = 10) => DateTime.Now.AddDays(-dias);

        [SetUp]
        public void Inicializar()
        {
            _mockRepositorio = new Mock<ITareaRepositorio>();
            _mockUsuarios = new Mock<IUsuarioRepositorio>();

            _mockUsuarios
                .Setup(u => u.ObtenerNombresPorIds(It.IsAny<IEnumerable<int>>()))
                .Returns(new Dictionary<int, string>());

            _servicio = new GestorTareasService(
                _mockRepositorio.Object,
                _mockUsuarios.Object,
                NullLogger<GestorTareasService>.Instance);
        }

        // ============================================================
        //  ObtenerTareas
        // ============================================================

        [Test]
        public void ObtenerTareas_DevuelveTodas_CuandoExistenTareas()
        {
            _mockRepositorio.Setup(r => r.ObtenerTodas())
                .Returns(new List<Tarea>
                {
                    new TareaSimple("Buenos dias", "Siempre es de dia", FechaFutura(1), USUARIO_ID),
                    new TareaSimple("Buenas tardes", "Esperando que sea de noche", FechaFutura(2), USUARIO_ID),
                    new TareaSimple("Buenas noches", "Una noche tranquila", FechaFutura(3), USUARIO_ID),
                    new TareaSimple("Un nuevo dia", "", FechaFutura(4), USUARIO_ID),
                    new TareaSimple("Desayunar", "Espero que sea facil", FechaFutura(5), USUARIO_ID),
                    new TareaSimple("Almuerzo", "Es de dia", FechaFutura(6), USUARIO_ID),
                    new TareaSimple("Comida", "Espero comer fuera", FechaFutura(7), USUARIO_ID),
                    new TareaSimple("Merienda", "Hoy se cena", FechaFutura(8), USUARIO_ID)
                });

            var resultado = _servicio.ObtenerTareas();

            Assert.That(resultado, Has.Count.EqualTo(8));
            Assert.That(resultado[0].Titulo, Is.EqualTo("Buenos dias"));
        }

        [Test]
        public void ObtenerTareas_DevuelveListaVacia_CuandoNoHayTareas()
        {
            _mockRepositorio.Setup(r => r.ObtenerTodas()).Returns(new List<Tarea>());

            var resultado = _servicio.ObtenerTareas();

            Assert.That(resultado, Is.Empty);
        }

        // ============================================================
        //  ObtenerPaginadas (filtro por rol)
        // ============================================================

        [Test]
        public void ObtenerPaginadas_DevuelveTodas_CuandoEsAdmin()
        {
            var tareas = new List<Tarea>
            {
                new TareaSimple("De usuario 1", "x", FechaFutura(), USUARIO_ID),
                new TareaSimple("De usuario 99", "y", FechaFutura(), OTRO_USUARIO_ID)
            };
            _mockRepositorio
                .Setup(r => r.ObtenerPaginadas(1, 10, null))
                .Returns((tareas, 2));

            var resultado = _servicio.ObtenerPaginadas(1, 10, null, USUARIO_ID, ES_ADMIN);

            Assert.That(resultado.Items, Has.Count.EqualTo(2));
            // Debe llamar al metodo "ver todas", no al filtrado
            _mockRepositorio.Verify(r => r.ObtenerPaginadas(1, 10, null), Times.Once);
            _mockRepositorio.Verify(
                r => r.ObtenerPaginadasPorUsuario(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()),
                Times.Never);
        }

        [Test]
        public void ObtenerPaginadas_DevuelveSoloLasDelUsuario_CuandoNoEsAdmin()
        {
            var tareas = new List<Tarea>
            {
                new TareaSimple("Solo mia", "x", FechaFutura(), USUARIO_ID)
            };
            _mockRepositorio
                .Setup(r => r.ObtenerPaginadasPorUsuario(1, 10, null, USUARIO_ID))
                .Returns((tareas, 1));

            var resultado = _servicio.ObtenerPaginadas(1, 10, null, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado.Items, Has.Count.EqualTo(1));
            _mockRepositorio.Verify(
                r => r.ObtenerPaginadasPorUsuario(1, 10, null, USUARIO_ID),
                Times.Once);
            _mockRepositorio.Verify(
                r => r.ObtenerPaginadas(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()),
                Times.Never);
        }

        // ============================================================
        //  ObtenerTareaPorId
        // ============================================================

        [Test]
        public void ObtenerTareaPorId_DevuelveTarea_CuandoEsAdmin()
        {
            var tarea = new TareaSimple("X", "y", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(5)).Returns(tarea);

            var resultado = _servicio.ObtenerTareaPorId(5, USUARIO_ID, ES_ADMIN);

            Assert.That(resultado, Is.Not.Null);
            Assert.That(resultado!.Titulo, Is.EqualTo("X"));
        }

        [Test]
        public void ObtenerTareaPorId_DevuelveTarea_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("Mia", "y", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(5)).Returns(tarea);

            var resultado = _servicio.ObtenerTareaPorId(5, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.Not.Null);
        }

        [Test]
        public void ObtenerTareaPorId_DevuelveNull_CuandoNoEsPropietarioNiAdmin()
        {
            var tarea = new TareaSimple("Ajena", "y", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(5)).Returns(tarea);

            var resultado = _servicio.ObtenerTareaPorId(5, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.Null);
        }

        [Test]
        public void ObtenerTareaPorId_DevuelveNull_CuandoNoExiste()
        {
            _mockRepositorio.Setup(r => r.ObtenerPorId(999)).Returns((Tarea?)null);

            var resultado = _servicio.ObtenerTareaPorId(999, USUARIO_ID, ES_ADMIN);

            Assert.That(resultado, Is.Null);
        }

        // ============================================================
        //  AgregarTareaDesdeDTO (creacion)
        // ============================================================

        [Test]
        public void AgregarTarea_LanzaExcepcion_CuandoTituloVacio()
        {
            var dto = new TareaDTO
            {
                Titulo = "",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            Assert.Throws<ArgumentException>(() => _servicio.AgregarTareaDesdeDTO(dto));
        }

        [Test]
        public void AgregarTarea_LanzaExcepcion_CuandoTipoInvalido()
        {
            var dto = new TareaDTO
            {
                Titulo = "X",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "inventado"
            };

            Assert.Throws<ArgumentException>(() => _servicio.AgregarTareaDesdeDTO(dto));
        }

        [Test]
        public void AgregarTarea_LanzaExcepcion_CuandoFechaEsPasada()
        {
            var dto = new TareaDTO
            {
                Titulo = "X",
                Descripcion = "",
                FechaLimite = FechaPasada(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            Assert.Throws<ArgumentException>(() => _servicio.AgregarTareaDesdeDTO(dto));
        }

        [Test]
        public void AgregarTarea_LlamaARepositorio_CuandoDatosCorrectos()
        {
            var dto = new TareaDTO
            {
                Titulo = "Buenos dias",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            _servicio.AgregarTareaDesdeDTO(dto);

            _mockRepositorio.Verify(r => r.Agregar(It.IsAny<TareaSimple>()), Times.Once);
        }

        [Test]
        public void AgregarTarea_CreaPomodoroConDuracionCorrecta()
        {
            TareaPomodoro? capturada = null;
            _mockRepositorio
                .Setup(r => r.Agregar(It.IsAny<TareaPomodoro>()))
                .Callback<Tarea>(t => capturada = (TareaPomodoro)t);

            var dto = new TareaDTO
            {
                Titulo = "Estudiar",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "pomodoro",
                DuracionMinutos = 45
            };

            _servicio.AgregarTareaDesdeDTO(dto);

            Assert.That(capturada, Is.Not.Null);
            Assert.That(capturada!.DuracionMinutos, Is.EqualTo(45));
        }

        [Test]
        public void AgregarTarea_CreaRecurrenteConIntervaloCorrecto()
        {
            TareaRecurrente? capturada = null;
            _mockRepositorio
                .Setup(r => r.Agregar(It.IsAny<TareaRecurrente>()))
                .Callback<Tarea>(t => capturada = (TareaRecurrente)t);

            var dto = new TareaDTO
            {
                Titulo = "Regar plantas",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "recurrente",
                IntervaloDias = 7
            };

            _servicio.AgregarTareaDesdeDTO(dto);

            Assert.That(capturada, Is.Not.Null);
            Assert.That(capturada!.IntervaloDias, Is.EqualTo(7));
        }

        // ============================================================
        //  ActualizarTarea
        // ============================================================

        [Test]
        public void ActualizarTarea_DevuelveOk_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("Vieja", "x", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var dto = new TareaDTO
            {
                Titulo = "Nueva",
                Descripcion = "nueva desc",
                FechaLimite = FechaFutura(10),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            var resultado = _servicio.ActualizarTarea(1, dto, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            _mockRepositorio.Verify(r => r.Actualizar(tarea), Times.Once);
        }

        [Test]
        public void ActualizarTarea_DevuelveProhibida_CuandoNoEsPropietario()
        {
            var tarea = new TareaSimple("Ajena", "x", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var dto = new TareaDTO
            {
                Titulo = "Nueva",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            var resultado = _servicio.ActualizarTarea(1, dto, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
            _mockRepositorio.Verify(r => r.Actualizar(It.IsAny<Tarea>()), Times.Never);
        }

        [Test]
        public void ActualizarTarea_DevuelveNoEncontrada_CuandoNoExiste()
        {
            _mockRepositorio.Setup(r => r.ObtenerPorId(999)).Returns((Tarea?)null);

            var dto = new TareaDTO
            {
                Titulo = "Nueva",
                Descripcion = "",
                FechaLimite = FechaFutura(),
                Estado = "Pendiente",
                UsuarioID = USUARIO_ID,
                TipoTarea = "simple"
            };

            var resultado = _servicio.ActualizarTarea(999, dto, USUARIO_ID, ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.NoEncontrada));
        }

        // ============================================================
        //  EliminarTarea
        // ============================================================

        [Test]
        public void EliminarTarea_DevuelveOk_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("X", "y", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.EliminarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            _mockRepositorio.Verify(r => r.Eliminar(tarea), Times.Once);
        }

        [Test]
        public void EliminarTarea_DevuelveProhibida_CuandoNoEsPropietario()
        {
            var tarea = new TareaSimple("Ajena", "y", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.EliminarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
            _mockRepositorio.Verify(r => r.Eliminar(It.IsAny<Tarea>()), Times.Never);
        }

        [Test]
        public void EliminarTarea_DevuelveNoEncontrada_CuandoNoExiste()
        {
            _mockRepositorio.Setup(r => r.ObtenerPorId(999)).Returns((Tarea?)null);

            var resultado = _servicio.EliminarTarea(999, USUARIO_ID, ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.NoEncontrada));
        }

        // ============================================================
        //  CompletarTarea
        // ============================================================

        [Test]
        public void CompletarTarea_DevuelveOk_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("X", "y", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.CompletarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Completada));
            _mockRepositorio.Verify(r => r.Actualizar(tarea), Times.Once);
        }

        [Test]
        public void CompletarTarea_GeneraSiguienteRecurrente_CuandoCompletaUnaRecurrente()
        {
            var recurrente = new TareaRecurrente("Diaria", "", FechaFutura(1), USUARIO_ID, 3);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(recurrente);

            var resultado = _servicio.CompletarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            // Se actualiza la original y se agrega una nueva recurrente con el mismo intervalo
            _mockRepositorio.Verify(r => r.Actualizar(recurrente), Times.Once);
            _mockRepositorio.Verify(
                r => r.Agregar(It.Is<TareaRecurrente>(t => t.IntervaloDias == 3 && t.Titulo == "Diaria")),
                Times.Once);
        }

        [Test]
        public void CompletarTarea_NoGeneraNuevaTarea_CuandoNoEsRecurrente()
        {
            var tarea = new TareaSimple("Simple", "", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            _servicio.CompletarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            _mockRepositorio.Verify(r => r.Agregar(It.IsAny<Tarea>()), Times.Never);
        }

        [Test]
        public void CompletarTarea_DevuelveProhibida_CuandoNoEsPropietario()
        {
            var tarea = new TareaSimple("Ajena", "", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.CompletarTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
        }

        // ============================================================
        //  CancelarTarea
        // ============================================================

        [Test]
        public void CancelarTarea_DevuelveOk_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("X", "", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.CancelarTarea(1, "porque si", USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            Assert.That(tarea.Estado, Is.EqualTo(EstadoTarea.Cancelada));
        }

        [Test]
        public void CancelarTarea_DevuelveProhibida_CuandoNoEsPropietario()
        {
            var tarea = new TareaSimple("Ajena", "", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.CancelarTarea(1, null, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
        }

        // ============================================================
        //  ReabrirTarea
        // ============================================================

        [Test]
        public void ReabrirTarea_DevuelveOk_CuandoEsPropietario()
        {
            var tarea = new TareaSimple("X", "", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.ReabrirTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            _mockRepositorio.Verify(r => r.Actualizar(tarea), Times.Once);
        }

        [Test]
        public void ReabrirTarea_DevuelveProhibida_CuandoNoEsPropietario()
        {
            var tarea = new TareaSimple("Ajena", "", FechaFutura(), OTRO_USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(tarea);

            var resultado = _servicio.ReabrirTarea(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
        }

        // ============================================================
        //  Pomodoro
        // ============================================================

        [Test]
        public void IniciarPomodoro_DevuelveOk_YMarcaTiempos()
        {
            var pomodoro = new TareaPomodoro("Estudiar", "", FechaFutura(), USUARIO_ID, 25);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(pomodoro);

            var resultado = _servicio.IniciarPomodoro(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            Assert.That(pomodoro.TiempoInit, Is.Not.Null);
            Assert.That(pomodoro.TiempoFinal, Is.Not.Null);
            // TiempoFinal = TiempoInit + 25 minutos
            Assert.That(
                (pomodoro.TiempoFinal!.Value - pomodoro.TiempoInit!.Value).TotalMinutes,
                Is.EqualTo(25).Within(0.01));
        }

        [Test]
        public void IniciarPomodoro_DevuelveProhibida_CuandoNoEsPomodoro()
        {
            var simple = new TareaSimple("X", "", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(simple);

            var resultado = _servicio.IniciarPomodoro(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
        }

        [Test]
        public void CompletarSesionPomodoro_IncrementaContadorYLimpiaTiempos()
        {
            var pomodoro = new TareaPomodoro("Estudiar", "", FechaFutura(), USUARIO_ID, 25);
            pomodoro.IniciarTemporizador();
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(pomodoro);

            var resultado = _servicio.CompletarSesionPomodoro(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Ok));
            Assert.That(pomodoro.Sesiones, Is.EqualTo(1));
            Assert.That(pomodoro.TiempoInit, Is.Null);
            Assert.That(pomodoro.TiempoFinal, Is.Null);
        }

        [Test]
        public void CompletarSesionPomodoro_DevuelveProhibida_CuandoNoEsPomodoro()
        {
            var simple = new TareaSimple("X", "", FechaFutura(), USUARIO_ID);
            _mockRepositorio.Setup(r => r.ObtenerPorId(1)).Returns(simple);

            var resultado = _servicio.CompletarSesionPomodoro(1, USUARIO_ID, NO_ES_ADMIN);

            Assert.That(resultado, Is.EqualTo(GestorTareasService.ResultadoOperacion.Prohibida));
        }
    }
}
