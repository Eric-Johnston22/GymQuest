using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateExerciseLogsForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_Id",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExerciseId",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_Id",
                table: "ExerciseLogs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ExerciseLogs");

            migrationBuilder.RenameColumn(
                name: "PlannedExerciseId",
                table: "ExerciseLogs",
                newName: "PlannedExercisesId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLogs_PlannedExerciseId",
                table: "ExerciseLogs",
                newName: "IX_ExerciseLogs_PlannedExercisesId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "ExerciseLogs",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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
                principalColumn: "PlannedExercisesId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_AspNetUsers_UserId",
                table: "ExerciseLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseLogs_PlannedExercises_PlannedExercisesId",
                table: "ExerciseLogs");

            migrationBuilder.DropIndex(
                name: "IX_ExerciseLogs_UserId",
                table: "ExerciseLogs");

            migrationBuilder.RenameColumn(
                name: "PlannedExercisesId",
                table: "ExerciseLogs",
                newName: "PlannedExerciseId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseLogs_PlannedExercisesId",
                table: "ExerciseLogs",
                newName: "IX_ExerciseLogs_PlannedExerciseId");

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

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseLogs_Id",
                table: "ExerciseLogs",
                column: "Id");

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
        }
    }
}
