-- Create the database
CREATE DATABASE WorkoutLogDB;
GO

USE WorkoutLogDB;
GO

-- Create Users table
CREATE TABLE Users (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    PasswordHash NVARCHAR(255) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE()
);
GO

-- Create WorkoutRoutines table
CREATE TABLE WorkoutRoutines (
    WorkoutRoutineId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    RoutineName NVARCHAR(100) NOT NULL,
    CycleDays INT NULL,
    IsCycle BIT NOT NULL,
    CreatedAt DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (UserId) REFERENCES Users(UserId)
);
GO

-- Create DaysOfWeek table
CREATE TABLE DaysOfWeek (
    DayId INT IDENTITY(1,1) PRIMARY KEY,
    DayName NVARCHAR(10) NOT NULL
);
GO

-- Insert days of the week
INSERT INTO DaysOfWeek (DayName) VALUES 
('Monday'), 
('Tuesday'), 
('Wednesday'), 
('Thursday'), 
('Friday'), 
('Saturday'), 
('Sunday');
GO

-- Create WorkoutDays table
CREATE TABLE WorkoutDays (
    WorkoutDayId INT IDENTITY(1,1) PRIMARY KEY,
    WorkoutRoutineId INT NOT NULL,
    DayInCycle INT NULL,
    DayId INT NULL,
    WorkoutType NVARCHAR(50) NOT NULL,
    FOREIGN KEY (WorkoutRoutineId) REFERENCES WorkoutRoutines(WorkoutRoutineId),
    FOREIGN KEY (DayId) REFERENCES DaysOfWeek(DayId)
);
GO

-- Create Exercises table
CREATE TABLE Exercises (
    ExerciseId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(MAX)
);
GO

-- Create PlannedExercises table
CREATE TABLE PlannedExercises (
    PlannedExerciseId INT IDENTITY(1,1) PRIMARY KEY,
    WorkoutDayId INT NOT NULL,
    ExerciseId INT NOT NULL,
    Sets INT NOT NULL,
    Reps INT NOT NULL,
    Weight DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(MAX),
    FOREIGN KEY (WorkoutDayId) REFERENCES WorkoutDays(WorkoutDayId),
    FOREIGN KEY (ExerciseId) REFERENCES Exercises(ExerciseId)
);
GO

-- Create ExerciseLogs table
CREATE TABLE ExerciseLogs (
    LogId INT IDENTITY(1,1) PRIMARY KEY,
    UserId INT NOT NULL,
    PlannedExerciseId INT NOT NULL,
    Date DATETIME NOT NULL,
    Sets INT NOT NULL,
    Reps INT NOT NULL,
    Weight DECIMAL(5,2) NOT NULL,
    Notes NVARCHAR(MAX),
    FOREIGN KEY (UserId) REFERENCES Users(UserId),
    FOREIGN KEY (PlannedExerciseId) REFERENCES PlannedExercises(PlannedExerciseId)
);
GO

-- Insert sample data

-- Users
INSERT INTO Users (FirstName, LastName, PasswordHash, Email, CreatedAt) VALUES
('John', 'Doe', 'hashed_password_1', 'john@example.com', '2024-07-01 10:00:00'),
('Jane', 'Smith', 'hashed_password_2', 'jane@example.com', '2024-07-02 11:00:00');
GO

-- WorkoutRoutines
INSERT INTO WorkoutRoutines (UserId, RoutineName, CycleDays, IsCycle, CreatedAt) VALUES
(1, 'Push-Pull-Legs', 4, 1, '2024-08-01 09:00:00'),
(2, 'Upper-Lower Split', NULL, 0, '2024-08-01 09:30:00');
GO

-- WorkoutDays for Push-Pull-Legs (Cycle Plan)
INSERT INTO WorkoutDays (WorkoutRoutineId, DayInCycle, DayId, WorkoutType) VALUES
(1, 1, NULL, 'Push'),
(1, 2, NULL, 'Pull'),
(1, 3, NULL, 'Legs'),
(1, 4, NULL, 'Rest');
GO

-- WorkoutDays for Upper-Lower Split (Set Weekly Schedule)
INSERT INTO WorkoutDays (WorkoutRoutineId, DayInCycle, DayId, WorkoutType) VALUES
(2, NULL, 1, 'Upper'), -- Monday
(2, NULL, 2, 'Lower'), -- Tuesday
(2, NULL, 4, 'Upper'), -- Thursday
(2, NULL, 5, 'Lower'); -- Friday
GO

-- Exercises
INSERT INTO Exercises (Name, Description) VALUES
('Barbell Bench Press', 'A basic chest exercise using a barbell.'),
('Incline Barbell Bench Press', 'An incline version of the barbell bench press.'),
('Overhead Dumbbell Press', 'A shoulder exercise using dumbbells.'),
('Tricep Extension', 'An isolation exercise for triceps.'),
('Squat', 'A basic lower body exercise using a barbell.');
GO

-- PlannedExercises for Push-Pull-Legs (Cycle Plan)
INSERT INTO PlannedExercises (WorkoutDayId, ExerciseId, Sets, Reps, Weight, Notes) VALUES
(1, 1, 4, 8, 135.00, 'Use a spotter for the last set.'),
(1, 2, 3, 8, 120.00, 'Incline to 30 degrees.'),
(1, 3, 3, 10, 50.00, 'Pause at the top.'),
(1, 4, 3, 15, 30.00, 'Use controlled movements.'),
(3, 5, 4, 8, 200.00, 'Keep back straight.');
GO

-- PlannedExercises for Upper-Lower Split (Set Weekly Schedule)
INSERT INTO PlannedExercises (WorkoutDayId, ExerciseId, Sets, Reps, Weight, Notes) VALUES
(5, 1, 4, 8, 135.00, 'Warm up with lighter weight.'),
(5, 2, 3, 8, 120.00, 'Focus on form.'),
(5, 3, 3, 10, 50.00, 'Avoid arching back.'),
(5, 4, 3, 15, 30.00, 'Do not lock elbows.'),
(6, 5, 4, 8, 200.00, 'Use a belt for support.');
GO

-- ExerciseLogs for User 1
INSERT INTO ExerciseLogs (UserId, PlannedExerciseId, Date, Sets, Reps, Weight, Notes) VALUES
(1, 1, '2024-08-05 07:00:00', 4, 8, 135.00, 'Completed all sets and reps.'),
(1, 2, '2024-08-05 07:30:00', 3, 8, 120.00, 'Completed all sets and reps.'),
(1, 3, '2024-08-05 08:00:00', 3, 10, 50.00, 'Struggled with the last set.'),
(1, 4, '2024-08-05 08:30:00', 3, 15, 30.00, 'Completed all sets and reps.'),
(1, 5, '2024-08-07 07:00:00', 4, 8, 200.00, 'Completed all sets and reps.');
GO
