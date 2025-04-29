using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OurNotesAppBackEnd.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProductImagePropToImageUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Products",
                newName: "ImageUrl");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageUrl",
                table: "Products",
                newName: "Image");
        }
    }
}
