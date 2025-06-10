using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurNotesAppBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class NotesAccessModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notes",
                type: "character varying(3000)",
                maxLength: 3000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "Notes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NoteAccesses",
                columns: table => new
                {
                    NoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    AppUserId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteAccesses", x => new { x.NoteId, x.AppUserId });
                    table.ForeignKey(
                        name: "FK_NoteAccesses_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_NoteAccesses_Notes_NoteId",
                        column: x => x.NoteId,
                        principalTable: "Notes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notes_AppUserId",
                table: "Notes",
                column: "AppUserId");

            migrationBuilder.CreateIndex(
                name: "IX_NoteAccesses_AppUserId",
                table: "NoteAccesses",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_AspNetUsers_AppUserId",
                table: "Notes",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notes_AspNetUsers_AppUserId",
                table: "Notes");

            migrationBuilder.DropTable(
                name: "NoteAccesses");

            migrationBuilder.DropIndex(
                name: "IX_Notes_AppUserId",
                table: "Notes");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "Notes");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Notes",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(3000)",
                oldMaxLength: 3000,
                oldNullable: true);
        }
    }
}
