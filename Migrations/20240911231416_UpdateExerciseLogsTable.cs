using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_UserId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExercisesId",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_PlannedExercisesId",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_UserId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "PlannedExercisesId",
                table: "ExerciseLogs");

            migrationBuilder.RenameColumn(
                name: "Sets",
                table: "ExerciseLogs",
                newName: "SetNumber");

            migrationBuilder.RenameColumn(
                name: "Reps",
                table: "ExerciseLogs",
                newName: "RepsCompleted");

            migrationBuilder.AddColumn<int>(
                name: "DaysOfWeekDayId",
                table: "WorkoutDays",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExerciseLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "ExerciseLogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsSuccessful",
                table: "ExerciseLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_DaysOfWeekDayId",
                table: "WorkoutDays",
                column: "DaysOfWeekDayId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_Id",
                table: "ExerciseLogs",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_PlannedExerciseId",
                table: "ExerciseLogs",
                column: "PlannedExerciseId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_Id",
                table: "ExerciseLogs",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExerciseId",
                table: "ExerciseLogs",
                column: "PlannedExerciseId",
                principalTable: "PlannedExercises",
                principalColumn: "PlannedExercisesId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DaysOfWeekDayId",
                table: "WorkoutDays",
                column: "DaysOfWeekDayId",
                principalTable: "DaysOfWeek",
                principalColumn: "DayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_Id",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DaysOfWeekDayId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_DaysOfWeekDayId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_Id",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_PlannedExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "DaysOfWeekDayId",
                table: "WorkoutDays");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExerciseLogs");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExerciseLogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlannedExercisesId",
                table: "ExerciseLogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_PlannedExercisesId",
                table: "ExerciseLogs",
                column: "PlannedExercisesId");

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_UserId",
                table: "ExerciseLogs",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_UserId",
                table: "ExerciseLogs",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExercisesId",
                table: "ExerciseLogs",
                column: "PlannedExercisesId",
                principalTable: "PlannedExercises",
                principalColumn: "PlannedExercisesId");
        }
    }
}
