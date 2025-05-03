using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace OurNotesAppBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelAndConfurationTwo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0821E65C-724B-4DC7-88DC-8E02775A4100", "0821E65C-724B-4DC7-88DC-8E02775A4100", "Admin", "ADMIN" },
                    { "6C4EE28F-7B94-4011-A0C0-6B08EFBEEA25", "6C4EE28F-7B94-4011-A0C0-6B08EFBEEA25", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0821E65C-724B-4DC7-88DC-8E02775A4100");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6C4EE28F-7B94-4011-A0C0-6B08EFBEEA25");
        }
    }
}
