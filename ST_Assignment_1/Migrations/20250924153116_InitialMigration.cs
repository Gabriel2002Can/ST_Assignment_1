using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ST_Assignment_1.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Category = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    DefaultSets = table.Column<int>(type: "integer", nullable: false),
                    DefaultReps = table.Column<string>(type: "text", nullable: false),
                    DefaultRestSeconds = table.Column<int>(type: "integer", nullable: false),
                    Difficulty = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Templates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    Frequency = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Templates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
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
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TemplateId = table.Column<int>(type: "integer", nullable: true),
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
                name: "TemplateItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TargetSets = table.Column<int>(type: "integer", nullable: false),
                    TargetReps = table.Column<string>(type: "text", nullable: false),
                    TargetRestSeconds = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateItems_CalendarEntries_Id",
                        column: x => x.Id,
                        principalTable: "CalendarEntries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateItems_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TemplateItems_Templates_Id",
                        column: x => x.Id,
                        principalTable: "Templates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    CalendarEntryId = table.Column<int>(type: "integer", nullable: true),
                    StartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Notes = table.Column<string>(type: "text", nullable: false)
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
                    SessionId = table.Column<int>(type: "integer", nullable: false),
                    ExerciseId = table.Column<int>(type: "integer", nullable: false),
                    Order = table.Column<int>(type: "integer", nullable: false),
                    TargetSets = table.Column<int>(type: "integer", nullable: false),
                    TargetReps = table.Column<string>(type: "text", nullable: false),
                    TargetRestSeconds = table.Column<int>(type: "integer", nullable: false),
                    CompletedSets = table.Column<int>(type: "integer", nullable: false)
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
                    SetNumber = table.Column<int>(type: "integer", nullable: false),
                    PlannedRepsOrDuration = table.Column<string>(type: "text", nullable: false),
                    ActualRepsOrDuration = table.Column<string>(type: "text", nullable: false),
                    RestAfterSetSeconds = table.Column<int>(type: "integer", nullable: false),
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

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "Id", "Category", "CreatedAt", "DefaultReps", "DefaultRestSeconds", "DefaultSets", "Description", "Difficulty", "Name" },
                values: new object[,]
                {
                    { 1, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-15", 75, 3, "Standard push-up. Keep body aligned.", 0, "Push-up" },
                    { 2, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-20", 60, 3, "Incline push-up.", 0, "Incline Push-up" },
                    { 3, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6-12", 90, 3, "Diamond push-up.", 1, "Diamond Push-up" },
                    { 4, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6-12", 105, 3, "Pike push-up.", 1, "Pike Push-up" },
                    { 5, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3-10", 135, 4, "Pull-up.", 1, "Pull-up" },
                    { 6, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3-10", 135, 4, "Chin-up.", 1, "Chin-up" },
                    { 7, 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-15", 75, 3, "Bodyweight row.", 0, "Bodyweight Row (Australian Pull-up)" },
                    { 8, 0, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6-15", 105, 3, "Parallel bar dips.", 1, "Dips (parallel bar)" },
                    { 9, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "12-20", 60, 3, "Bodyweight squat.", 0, "Squat (bodyweight)" },
                    { 10, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-15", 75, 3, "Jump squat.", 1, "Jump Squat" },
                    { 11, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "3-8", 135, 4, "Assisted pistol squat.", 2, "Pistol Squat (assisted)" },
                    { 12, 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-15", 75, 3, "Lunges.", 0, "Lunges (walking/static)" },
                    { 13, 3, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "30-90s", 60, 3, "Plank.", 0, "Plank" },
                    { 14, 3, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "8-15", 75, 3, "Hanging leg raise.", 1, "Hanging Leg Raise" },
                    { 15, 4, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "30s", 45, 3, "Mountain climbers.", 0, "Mountain Climbers" },
                    { 16, 4, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "6-15", 90, 3, "Burpee.", 1, "Burpee" }
                });

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
                name: "IX_TemplateItems_ExerciseId",
                table: "TemplateItems",
                column: "ExerciseId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_CalendarEntryId",
                table: "WorkoutSessions",
                column: "CalendarEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutSessions_UserId",
                table: "WorkoutSessions",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetRecords");

            migrationBuilder.DropTable(
                name: "TemplateItems");

            migrationBuilder.DropTable(
                name: "SessionExercises");

            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "WorkoutSessions");

            migrationBuilder.DropTable(
                name: "CalendarEntries");

            migrationBuilder.DropTable(
                name: "Templates");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
