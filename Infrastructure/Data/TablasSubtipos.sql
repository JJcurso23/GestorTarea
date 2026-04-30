USE [GestorTarea]
GO

-- 1. Tarea Simple (Anotaciones rápidas)
CREATE TABLE TareasSimples (
    TareaID INT PRIMARY KEY,
    NotasAdicionales VARCHAR(MAX),
    CONSTRAINT FK_Simple_Tarea FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID)
);

-- 2. Tarea Recurrente (Frecuencias)
CREATE TABLE TareasRecurrentes (
    TareaID INT PRIMARY KEY,
    Frecuencia VARCHAR(50), -- Ejemplo: 'Diaria', 'Semanal'
    DiaDeLaSemana INT,
    CONSTRAINT FK_Recurrente_Tarea FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID)
);

-- 3. Tarea Urgente (Prioridades críticas)
CREATE TABLE TareasUrgentes (
    TareaID INT PRIMARY KEY,
    FechaLimite DATETIME NOT NULL,
    NivelPrioridad INT, -- 1 al 5
    CONSTRAINT FK_Urgente_Tarea FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID)
);

-- 4. Tarea Pomodoro (Enfocada en tiempo)
CREATE TABLE TareasPomodoro (
    TareaID INT PRIMARY KEY,
    CantidadSesiones INT DEFAULT 1,
    DuracionMinutos INT DEFAULT 25,
    CONSTRAINT FK_Pomodoro_Tarea FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID)
);

-- 5. Tarea Con Tarea (Subtareas o Dependencias)
-- Esta tabla vincula una tarea con otra tarea que debe hacerse antes
CREATE TABLE TareasConTarea (
    TareaID INT PRIMARY KEY,
    TareaDependienteID INT NOT NULL,
    InstruccionesDependencia TEXT,
    CONSTRAINT FK_ConTarea_TareaPrincipal FOREIGN KEY (TareaID) REFERENCES Tareas(TareaID),
    CONSTRAINT FK_ConTarea_TareaDependencia FOREIGN KEY (TareaDependienteID) REFERENCES Tareas(TareaID)
);