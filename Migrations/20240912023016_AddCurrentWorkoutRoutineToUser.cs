using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class AddCurrentWorkoutRoutineToUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutRoutines_AspNetUsers_UserId",
                table: "WorkoutRoutines");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutRoutines_UserId",
                table: "WorkoutRoutines");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkoutRoutines",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrentWorkoutRoutineId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CurrentWorkoutRoutineId",
                table: "AspNetUsers",
                column: "CurrentWorkoutRoutineId",
                unique: true,
                filter: "[CurrentWorkoutRoutineId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_WorkoutRoutines_CurrentWorkoutRoutineId",
                table: "AspNetUsers",
                column: "CurrentWorkoutRoutineId",
                principalTable: "WorkoutRoutines",
                principalColumn: "WorkoutRoutineId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_WorkoutRoutines_CurrentWorkoutRoutineId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CurrentWorkoutRoutineId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CurrentWorkoutRoutineId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "WorkoutRoutines",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutRoutines_UserId",
                table: "WorkoutRoutines",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutRoutines_AspNetUsers_UserId",
                table: "WorkoutRoutines",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
