# GymQuest - ASP.NET Workout Tracker

## Overview
GymQuest is a full-featured workout tracking application built using ASP.NET Core MVC. It allows users to create workout routines, log exercises, and track their progress over time. Users can also dynamically add exercises to their routines and manage their daily workouts with ease. The application leverages an Azure SQL database to store workout data.

**Live Application:** <a href="https://gymquest.azurewebsites.net/" target="_blank">GymQuest - Azure Deployment</a>

## Features
- **User Registration and Authentication**: Users can sign up, log in, and manage their account.
- **Workout Routines**: Create, view, and manage workout routines.
- **Dynamic Exercise Management**: Add, edit, and delete exercises from routines, even during active workouts.
- **Progress Tracking**: Log completed exercises and view past workout data.
- **Azure SQL Integration**: Data persistence using a scalable Azure SQL Database.

## User Guide

#### Getting Started
- **Registration**: Navigate to the <a href="https://gymquest.azurewebsites.net/account/register" target="_blank">Register page</a> to create an account.
- **Logging In**: Use your credentials on the <a href="https://gymquest.azurewebsites.net/account/login" target="_blank">Login page</a> to access your dashboard.

#### Managing Workout Routines
- **Create a Workout Routine**: Once logged in, go to the "Create Routine" page, enter the routine name, add days to your routine, and add exercises to each day by selecting from the dropdown.
- **Start a Routine**: Once you've created a routine, you will be directed to the "View Routine" page where you can either start or make further changes to your routine
- **Log Completed Workouts**: During your workout, you can actively log your completed exercises by inputting your sets, reps, and weights. Once you've completed your workout, you will be redirected to a workout summary page.

## Architecture & Design
### Architecture
- **Frontend**: ASP.NET Core MVC + Bootstrap for responsive UI
- **Backend**: ASP.NET Core, Entity Framework Core for data access
- **Database**: Azure SQL
- **Security**: ASP.NET Core Identity for authentication

### Entity-Relationship Diagram (ERD)
![GymQuest ERD](./diagrams/gymquest-erd.png)
The database contains several key tables:
- `Users`: Stores user information.
- `WorkoutRoutines`: Stores routine metadata.
- `WorkoutDays`: Stores days associated with routines.
- `PlannedExercises`: Contains details of exercises planned for each day.

### Code Structure - MVC
- **Controllers**: Handles the logic for managing users, workouts, and exercises.
- **Models**: Defines the data structure and represents entities like users, workouts, and exercises.
- **Views**: Razor Views for rendering HTML pages to the user.

### Key Components
- **WorkoutController.cs**: Handles all actions related to workout routines and exercises.
- **WorkoutService.cs**: Contains business logic for managing workouts.
- **WorkoutRepository.cs**: Interacts with the Azure SQL database to retrieve and persist data.

## Code Snippet

### Example: Adding an Exercise to a Workout Day

```csharp
public async Task<IActionResult> AddExerciseToDay(int workoutRoutineId, string dayName, int exerciseId, int sets, int reps, decimal weight, string? notes)
{
    var workoutDay = await _workoutService.GetOrCreateWorkoutDayAsync(workoutRoutineId, dayName);
    if (workoutDay == null) return NotFound();

    var newExercise = new PlannedExercises
    {
        WorkoutDayId = workoutDay.WorkoutDayId,
        ExerciseId = exerciseId,
        Sets = sets,
        Reps = reps,
        Weight = weight,
        Notes = notes
    };

    await _workoutService.AddPlannedExerciseAsync(newExercise);
    return Ok(new { plannedExerciseId = newExercise.PlannedExercisesId });
}
```
