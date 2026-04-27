
-- Crear la base de datos --

if NOT EXISTS(
	SELECT name FROM sys.databases
	WHERE name = 'GestorTarea'
)
BEGIN
	CREATE DATABASE GestorTarea;
END
GO

USE GestorTarea;
GO

-- Tabla Usuario
CREATE TABLE Usuarios(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Nombre NVARCHAR(100) NOT NULL,
	Email NVARCHAR(200) NOT NULL,
	Edad INT NULL,
	EsAdmin BIT NOT NULL DEFAULT 0,
	CONSTRAINT UQ_Usuarios_Email UNIQUE(Email)
);
GO


-- Tabla Simple
CREATE TABLE Tareas(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	TipoTarea NVARCHAR(50) NOT NULL,
	Titulo NVARCHAR(200) NOT NULL,
	Descripcion NVARCHAR(500) NULL,
	FechaLimite DATETIME2 NULL,
	EstaCompleta BIT NOT NULL DEFAULT 0,
	FechaCreacion DATETIME2 NOT NULL DEFAULT GETDATE(),
	UsuarioId INT NOT NULL,
	CONSTRAINT FK_Tareas_Usuarios
		FOREIGN KEY(UsuarioId) REFERENCES Usuarios(Id)
);
GO
-- Tabla 
-- Tabla Tarea Recurrente
CREATE TABLE TareasSimple(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Titulo NVARCHAR(200) NOT NULL,
	Descripcion NVARCHAR(500) NULL,
	FechaLimite DATETIME2 NULL,
	EstaCompleta BIT NOT NULL DEFAULT 0,
	IntervalosDias NVARCHAR(10) NOT NULL,
	CONSTRAINT FK_Tareas_Usuarios
		FOREIGN KEY(UsuarioId) REFERENCES Usuarios(Id)
);
GO

-- Tabla Tarea Urgente
CREATE TABLE TareaUrgente(
	Id INT IDENTITY(1,1) PRIMARY KEY,
	Titulo NVARCHAR(200) NOT NULL,
	Descripcion NVARCHAR(500) NULL,
	FechaLimite DATETIME 

);
-- Tabla Tarea Pomodoro
-- Tabla Tarea con Tarea
-- Insertar un usuario
INSERT INTO Usuarios(Nombre, Email, Edad, EsAdmin)
VALUES('Ana Banana', 'banana@gestor.com', 39, 1);
GO

INSERT INTO Usuarios(Nombre, Email, Edad, EsAdmin)
VALUES('Tomas Tren', 'tremos@gestor.com', 29, 0);

GO
-- Insertar una tabla
INSERT INTO Tareas(TipoTarea, Titulo, FechaLimite, UsuarioId)
VALUES('Simple', 'Preparar informe trimestral', '2025-06-30', 1);