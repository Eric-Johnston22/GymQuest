using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymQuest.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DaysOfWeekDayId",
                table: "WorkoutDays");

            migrationBuilder.DropIndex(
                name: "IX_WorkoutDays_DaysOfWeekDayId",
                table: "WorkoutDays");

            migrationBuilder.DropColumn(
                name: "DaysOfWeekDayId",
                table: "WorkoutDays");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DaysOfWeekDayId",
                table: "WorkoutDays",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_DaysOfWeekDayId",
                table: "WorkoutDays",
                column: "DaysOfWeekDayId");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkoutDays_DaysOfWeek_DaysOfWeekDayId",
                table: "WorkoutDays",
                column: "DaysOfWeekDayId",
                principalTable: "DaysOfWeek",
                principalColumn: "DayId");
        }
    }
}
