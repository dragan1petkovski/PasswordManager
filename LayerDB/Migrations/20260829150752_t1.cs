using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LayerDB.Migrations
{
    /// <inheritdoc />
    public partial class t1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "domain",
                table: "Credentials");

            migrationBuilder.DropColumn(
                name: "email",
                table: "Credentials");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "domain",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "email",
                table: "Credentials",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
