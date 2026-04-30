USE [GestorTarea]
GO

-- Tabla Base: Tareas
CREATE TABLE Tareas (
    TareaID INT PRIMARY KEY IDENTITY(1,1),
    UsuarioID INT NOT NULL,
    Titulo VARCHAR(200) NOT NULL,
    Descripcion TEXT,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Completada BIT DEFAULT 0,
    CONSTRAINT FK_Tareas_Usuarios FOREIGN KEY (UsuarioID) REFERENCES Usuarios(UsuarioID)
);