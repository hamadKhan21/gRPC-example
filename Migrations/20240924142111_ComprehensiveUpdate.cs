using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Discount.Grpc.Migrations
{
    /// <inheritdoc />
    public partial class ComprehensiveUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Coupones",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Coupones",
                keyColumn: "Id",
                keyValue: 1,
                column: "IsActive",
                value: false);

            migrationBuilder.UpdateData(
                table: "Coupones",
                keyColumn: "Id",
                keyValue: 2,
                column: "IsActive",
                value: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Coupones");
        }
    }
}
