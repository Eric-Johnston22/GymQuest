using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseLogsModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlannedExercises_Exercises_ExercisesExerciseId",
                table: "PlannedExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedExercises_WorkoutDays_WorkoutDaysWorkoutDayId",
                table: "PlannedExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_PlannedExercises_ExercisesExerciseId",
                table: "PlannedExercises");

            migrationBuilder.DropIndex(
                name: "IX_PlannedExercises_WorkoutDaysWorkoutDayId",
                table: "PlannedExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropColumn(
                name: "ExercisesExerciseId",
                table: "PlannedExercises");

            migrationBuilder.DropColumn(
                name: "WorkoutDaysWorkoutDayId",
                table: "PlannedExercises");

            migrationBuilder.RenameColumn(
                name: "Sets",
                table: "ExerciseLogs",
                newName: "SetNumber");

            migrationBuilder.RenameColumn(
                name: "Reps",
                table: "ExerciseLogs",
                newName: "RepsCompleted");

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "ExerciseLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_DayId",
                table: "WorkoutDays",
                column: "DayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedExercises_ExerciseId",
                table: "PlannedExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedExercises_WorkoutDayId",
                table: "PlannedExercises",
                column: "WorkoutDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedExercises_Exercises_ExerciseId",
                table: "PlannedExercises",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedExercises_WorkoutDays_WorkoutDayId",
                table: "PlannedExercises",
                column: "WorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DayId",
                table: "WorkoutDays",
                column: "DayId",
                principalTable: "DaysOfWeek",
                principalColumn: "DayId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutineId",
                principalTable: "WorkoutRoutines",
                principalColumn: "WorkoutRoutineId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlannedExercises_Exercises_ExerciseId",
                table: "PlannedExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_PlannedExercises_WorkoutDays_WorkoutDayId",
                table: "PlannedExercises");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DayId",
                table: "WorkoutDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_DayId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_WorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_PlannedExercises_ExerciseId",
                table: "PlannedExercises");

            migrationBuilder.DropIndex(
                name: "IX_PlannedExercises_WorkoutDayId",
                table: "PlannedExercises");

            migrationBuilder.DropColumn(
                name: "IsSuccessful",
                table: "ExerciseLogs");

            migrationBuilder.RenameColumn(
                name: "SetNumber",
                table: "ExerciseLogs",
                newName: "Sets");

            migrationBuilder.RenameColumn(
                name: "RepsCompleted",
                table: "ExerciseLogs",
                newName: "Reps");

            migrationBuilder.AddColumn<int>(
                name: "WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExercisesExerciseId",
                table: "PlannedExercises",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutDaysWorkoutDayId",
                table: "PlannedExercises",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutinesWorkoutRoutineId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedExercises_ExercisesExerciseId",
                table: "PlannedExercises",
                column: "ExercisesExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_PlannedExercises_WorkoutDaysWorkoutDayId",
                table: "PlannedExercises",
                column: "WorkoutDaysWorkoutDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedExercises_Exercises_ExercisesExerciseId",
                table: "PlannedExercises",
                column: "ExercisesExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlannedExercises_WorkoutDays_WorkoutDaysWorkoutDayId",
                table: "PlannedExercises",
                column: "WorkoutDaysWorkoutDayId",
                principalTable: "WorkoutDays",
                principalColumn: "WorkoutDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutinesWorkoutRoutineId",
                principalTable: "WorkoutRoutines",
                principalColumn: "WorkoutRoutineId");
        }
    }
}
