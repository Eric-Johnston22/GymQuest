using GymQuest.Data;
using GymQuest.Models;
using GymQuest.Models.ViewModels;
using static GymQuest.Models.ViewModels.CreateRoutineViewModel;

namespace GymQuest.Services
{
    public class WorkoutService
    {
        private readonly WorkoutRepository _workoutRepository;

        public WorkoutService(WorkoutRepository workoutRepository)
        {
            _workoutRepository = workoutRepository;
        }

        public async Task<int> CreateRoutineAsync(CreateRoutineViewModel model, string userId)
        {
            var workoutRoutine = new WorkoutRoutines
            {
                RoutineName = model.RoutineName,
                CycleDays = model.CycleDays,
                IsCycle = model.IsCycle,
                UserId = userId
            };

            foreach (var dayModel in model.WorkoutDays)
            {
                var dayOfWeek = _workoutRepository.GetDayOfWeekByName(dayModel.DayName);
                var workoutDay = new WorkoutDays
                {
                    WorkoutRoutineId = workoutRoutine.WorkoutRoutineId,
                    DayInCycle = model.WorkoutDays.IndexOf(dayModel) + 1,
                    DayId = dayOfWeek.DayId,
                    WorkoutType = "Your Workout Type"
                };

                foreach (var exerciseModel in dayModel.PlannedExercises)
                {
                    var plannedExercise = new PlannedExercises
                    {
                        ExerciseId = exerciseModel.ExerciseId,
                        Sets = exerciseModel.Sets,
                        Reps = exerciseModel.Reps,
                        Weight = exerciseModel.Weight,
                        Notes = exerciseModel.Notes
                    };
                    workoutDay.PlannedExercises.Add(plannedExercise);
                }

                workoutRoutine.WorkoutDays.Add(workoutDay);
            }

            await _workoutRepository.AddWorkoutRoutineAsync(workoutRoutine);
            return workoutRoutine.WorkoutRoutineId; // return the ID of the created routine
        }

        public List<Exercises> GetAllExercises()
        {
            return _workoutRepository.GetAllExercises();
        }
    }
}
