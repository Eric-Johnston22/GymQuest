using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Security.Claims;
using static GymQuest.Models.ViewModels.CreateRoutineViewModel;

namespace GymQuest.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;
        private readonly UserManager<User> _userManager;

        public WorkoutService(WorkoutRepository workoutRepository, UserManager<User> userManager)
        {
            _workoutRepository = workoutRepository;
            _userManager = userManager;
        }

        //public async Task<int> CreateRoutineAsync(CreateRoutineViewModel model, ClaimsPrincipal user)
        //{

        //    try
        //    {
        //        var userId = _userManager.GetUserId(user);
        //        var workoutRoutine = new WorkoutRoutines
        //        {
        //            RoutineName = model.RoutineName,
        //            CycleDays = model.CycleDays,
        //            IsCycle = model.IsCycle,
        //            UserId = userId,
        //            CreatedAt = DateTime.Now,
        //            Status = "Draft" // Set as draft
        //        };

        //        return await _workoutRepository.CreateWorkoutRoutineAsync(workoutRoutine);
        //    }
        //    catch (DbUpdateException ex)
        //    {
        //        throw new Exception("There was an error saving the workout routine", ex);
        //    }
        //}

        // New Updated method (overload)
        public async Task CreateRoutineAsync(WorkoutRoutines routine, string userId)
        {
            // Set the userId in the entity
            routine.UserId = userId;

            // Call the repository to save the routine
            await _workoutRepository.SaveRoutineAsync(routine);
        }



        public async Task AddWorkoutDaysAsync(AddWorkoutDaysViewModel model)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(model.WorkoutRoutineId);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                foreach (var day in model.WorkoutDays)
                {
                    workoutRoutine.WorkoutDays.Add(new WorkoutDays
                    {
                        DayId = day.DayId,
                        WorkoutRoutineId = model.WorkoutRoutineId,
                        WorkoutType = day.WorkoutType
                    });
                }

                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task AssignExercisesAsync(AssignExercisesViewModel model)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(model.WorkoutRoutineId);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                foreach (var day in model.WorkoutDays)
                {
                    var workoutDay = workoutRoutine.WorkoutDays.First(wd => wd.WorkoutDayId == day.WorkoutDayId);

                    foreach (var exercise in day.PlannedExercises)
                    {
                        workoutDay.PlannedExercises.Add(new PlannedExercises
                        {
                            ExerciseId = exercise.ExerciseId,
                            Sets = exercise.Sets,
                            Reps = exercise.Reps,
                            Weight = exercise.Weight,
                            Notes = exercise.Notes
                        });
                    }
                }

                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task CreateExercise(Exercises exercise)
        {
            await _workoutRepository.CreateExercise(exercise);
        }

        public async Task<(bool Success, string ExerciseName, string ErrorMessage)> AddExerciseToDayAsync(
            AssignExercisesViewModel.WorkoutDayExercisesViewModel.PlannedExerciseViewModel model, int workoutDayId)
        {
            try
            {
                // Business logic: validate inputs, check if the workout day exists, etc.
                var workoutDay = await _workoutRepository.GetWorkoutDayByIdAsync(workoutDayId);
                if (workoutDay == null)
                {
                    return (false, null, "Workout day not found.");
                }

                var exercise = await _workoutRepository.GetExerciseByIdAsync(model.ExerciseId);
                if (exercise == null)
                {
                    return (false, null, "Exercise not found.");
                }

                var plannedExercise = new PlannedExercises
                {
                    WorkoutDayId = workoutDayId,
                    ExerciseId = model.ExerciseId,
                    Sets = model.Sets,
                    Reps = model.Reps,
                    Weight = model.Weight,
                    Notes = model.Notes
                };

                // Delegate the actual database save operation to the data layer
                await _workoutRepository.AddPlannedExerciseAsync(plannedExercise);

                return (true, exercise.Name, null);
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine("Error in AddExerciseToDayAsync: " + ex.Message);
                return (false, null, "An error occurred while adding the exercise.");
            }
        }

        //public async Task AddExerciseToDayAsync(int workoutRoutineId, string dayName, int exerciseId, int sets, int reps, decimal weight, string? notes)
        //{
        //    // Fetch the workout routine by its ID
        //    var routine = await _workoutRepository.GetWorkoutRoutineByIdAsync(workoutRoutineId);

        //    if (routine == null)
        //    {
        //        throw new Exception("Routine not found.");
        //    }

        //    // Map the dayName to the corresponding DayId
        //    var dayId = GetDayIdFromDayName(dayName);

        //    // Check if the day already exists in the routine
        //    // Try to find the day by DayId
        //    var workoutDay = routine.WorkoutDays.FirstOrDefault(d => d.DayId == dayId);

        //    if (workoutDay == null)
        //    {

        //        // Create a new day for the routine
        //        workoutDay = new WorkoutDays
        //        {
        //            DayId = dayId,
        //            WorkoutRoutineId = workoutRoutineId,
        //            WorkoutType = "General",  // Default value; adjust as needed
        //            PlannedExercises = new List<PlannedExercises>()
        //        };

        //        // Add the new day to the routine
        //        routine.WorkoutDays.Add(workoutDay);
        //    }

        //    // Add the exercise to the day
        //    var plannedExercise = new PlannedExercises
        //    {
        //        ExerciseId = exerciseId,
        //        Sets = sets,
        //        Reps = reps,
        //        Weight = weight,
        //        Notes = notes
        //    };

        //    workoutDay.PlannedExercises.Add(plannedExercise);

        //    // Update the routine with the new day and exercise
        //    await _workoutRepository.UpdateWorkoutRoutineAsync(routine);
        //}

        public async Task<bool> AddCurrentDayToRoutineAsync(int workoutRoutineId, string dayName)
        {
            // Call the repository to check if the day exists or create a new one
            var existingDay = await _workoutRepository.GetWorkoutDayByRoutineAndDayNameAsync(workoutRoutineId, dayName);

            // If the day already exists, return false (or true, depending on your logic)
            if (existingDay != null)
            {
                return true; // Day already exists
            }

            // Fetch the DayId for the current day (assuming DayId corresponds to the day name)
            var dayId = GetDayIdFromDayName(dayName);

            // Create a new WorkoutDay for the routine
            var newWorkoutDay = new WorkoutDays
            {
                WorkoutRoutineId = workoutRoutineId,
                DayId = dayId,
                WorkoutType = "General", // Or set this dynamically based on user input
                PlannedExercises = new List<PlannedExercises>() // Empty list for now
            };

            // Add the day to the routine
            await _workoutRepository.AddWorkoutDayAsync(newWorkoutDay);

            return true;
        }

        // Helper function to get DayId based on the day name
        private int GetDayIdFromDayName(string dayName)
        {
            return dayName.ToLower() switch
            {
                "monday" => 1,
                "tuesday" => 2,
                "wednesday" => 3,
                "thursday" => 4,
                "friday" => 5,
                "saturday" => 6,
                "sunday" => 7,
                _ => throw new ArgumentException("Invalid day name", nameof(dayName)),
            };
        }



        public async Task<WorkoutRoutines?> GetWorkoutRoutineByIdAsync(int? id)
        {
            if (id == null)
            {
                return null;
            }
            return await _workoutRepository.GetWorkoutRoutineByIdAsync(id);
        }

        public async Task<List<WorkoutRoutines>> GetWorkoutRoutinesByUserAsync(string id)
        {
            return await _workoutRepository.GetWorkoutRoutinesByUserAsync(id);
        }

        public async Task CompleteRoutineAsync(int id)
        {
            var workoutRoutine = await _workoutRepository.GetWorkoutRoutineByIdAsync(id);
            if (workoutRoutine != null && workoutRoutine.Status == "Draft")
            {
                workoutRoutine.Status = "Completed";
                await _workoutRepository.UpdateWorkoutRoutineAsync(workoutRoutine);
            }
        }

        public async Task<List<Exercises>> GetExercisesAsync()
        {
            return await _workoutRepository.GetExercisesAsync();
        }

        public async Task UpdatePlannedExerciseAsync(PlannedExercises plannedExercise)
        {
            await _workoutRepository.UpdatePlannedExerciseAsync(plannedExercise);
        }

        public async Task<PlannedExercises> GetPlannedExerciseByIdAsync(int plannedExercisesId)
        {
            return await _workoutRepository.GetPlannedExerciseByIdAsync(plannedExercisesId);
        }


        public async Task<bool> RemovePlannedExerciseAsync(int plannedExercisesId)
        {
            return await _workoutRepository.RemovePlannedExerciseAsync(plannedExercisesId);
        }

        public async Task AddPlannedExerciseAsync(PlannedExercises plannedExercise)
        {
            await _workoutRepository.AddPlannedExerciseAsync(plannedExercise);
        }


    }
}
