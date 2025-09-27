using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ST_Assignment_1.Migrations
{
    /// <inheritdoc />
    public partial class initialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateItems_CalendarEntries_Id",
                table: "TemplateItems");

            migrationBuilder.DropForeignKey(
                name: "FK_TemplateItems_Templates_Id",
                table: "TemplateItems");

            migrationBuilder.DropTable(
                name: "SetRecords");

            migrationBuilder.DropTable(
                name: "SessionExercises");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "CalendarEntries");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DeleteData(
                table: "TemplateItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TemplateItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "TemplateId",
                table: "TemplateItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AppSettings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CurrentTemplateId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSettings", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "CurrentTemplateId" },
                values: new object[] { 1, 1 });

            migrationBuilder.UpdateData(
                table: "TemplateItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Notes", "TargetReps", "TargetRestSeconds", "TemplateId" },
                values: new object[] { "Push-ups: 10 reps, 3 sets, 5 min rest", "10", 300, 1 });

            migrationBuilder.UpdateData(
                table: "TemplateItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExerciseId", "Notes", "TargetReps", "TargetRestSeconds", "TemplateId" },
                values: new object[] { 2, "Squats: 15 reps, 3 sets, 5 min rest", "15", 300, 1 });

            migrationBuilder.UpdateData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "A simple full body routine.", "Full Body Example" });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateItems_TemplateId",
                table: "TemplateItems",
                column: "TemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateItems_Templates_TemplateId",
                table: "TemplateItems",
                column: "TemplateId",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TemplateItems_Templates_TemplateId",
                table: "TemplateItems");

            migrationBuilder.DropTable(
                name: "AppSettings");

            migrationBuilder.DropIndex(
                name: "IX_TemplateItems_TemplateId",
                table: "TemplateItems");

            migrationBuilder.DropColumn(
                name: "TemplateId",
                table: "TemplateItems");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "TemplateItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CalendarEntries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TemplateId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarEntries", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Templates_TemplateId",
                        column: x => x.TemplateId,
                        principalTable: "Templates",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CalendarEntries_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CalendarEntryId = table.Column<int>(type: "integer", nullable: true),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: false),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_CalendarEntries_CalendarEntryId",
                        column: x => x.CalendarEntryId,
                        principalTable: "CalendarEntries",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkoutSessions_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionExercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    CompletedSets = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TargetReps = table.Column<string>(type: "text", nullable: false),
                    TargetRestSeconds = table.Column<int>(type: "integer", nullable: false),
                    TargetSets = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionExercises", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionExercises_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionExercises_WorkoutSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "WorkoutSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SetRecords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SessionExerciseId = table.Column<int>(type: "integer", nullable: false),
                    ActualRepsOrDuration = table.Column<string>(type: "text", nullable: false),
                    PlannedRepsOrDuration = table.Column<string>(type: "text", nullable: false),
                    RestAfterSetSeconds = table.Column<int>(type: "integer", nullable: false),
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    Timestamp = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRecords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SetRecords_SessionExercises_SessionExerciseId",
                        column: x => x.SessionExerciseId,
                        principalTable: "SessionExercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "TemplateItems",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Notes", "TargetReps", "TargetRestSeconds" },
                values: new object[] { "Standard push-up", "8-12", 75 });

            migrationBuilder.UpdateData(
                table: "TemplateItems",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ExerciseId", "Notes", "TargetReps", "TargetRestSeconds" },
                values: new object[] { 9, "Bodyweight squat", "12-20", 60 });

            migrationBuilder.InsertData(
                table: "TemplateItems",
                columns: new[] { "Id", "ExerciseId", "Notes", "Order", "TargetReps", "TargetRestSeconds", "TargetSets" },
                values: new object[] { 3, 13, "Plank", 3, "30-60s", 60, 3 });

            migrationBuilder.UpdateData(
                table: "Templates",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Name" },
                values: new object[] { "A simple full body routine for beginners.", "Full Body Beginner" });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "Email", "PasswordHash", "Username" },
                values: new object[] { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "demo@example.com", "", "demo" });

            migrationBuilder.InsertData(
                table: "CalendarEntries",
                columns: new[] { "Id", "Date", "Status", "TemplateId", "UserId" },
                values: new object[] { 1, new DateTime(2024, 1, 2, 0, 0, 0, 0, DateTimeKind.Utc), 0, 1, 1 });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_TemplateId",
                table: "CalendarEntries",
                column: "TemplateId");

            migrationBuilder.CreateIndex(
                name: "IX_CalendarEntries_UserId",
                table: "CalendarEntries",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_ExerciseId",
                table: "SessionExercises",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_SessionExercises_SessionId",
                table: "SessionExercises",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRecords_SessionExerciseId",
                table: "SetRecords",
                column: "SessionExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_CalendarEntryId",
                table: "WorkoutSessions",
                column: "CalendarEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_UserId",
                table: "WorkoutSessions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateItems_CalendarEntries_Id",
                table: "TemplateItems",
                column: "Id",
                principalTable: "CalendarEntries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TemplateItems_Templates_Id",
                table: "TemplateItems",
                column: "Id",
                principalTable: "Templates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
