using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class AddStatusToWorkoutRoutine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "WorkoutRoutines",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutinesWorkoutRoutineId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays",
                column: "WorkoutRoutinesWorkoutRoutineId",
                principalTable: "WorkoutRoutines",
                principalColumn: "WorkoutRoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_WorkoutRoutines_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "WorkoutRoutines");

            migrationBuilder.DropColumn(
                name: "WorkoutRoutinesWorkoutRoutineId",
                table: "WorkoutDays");
        }
    }
}
