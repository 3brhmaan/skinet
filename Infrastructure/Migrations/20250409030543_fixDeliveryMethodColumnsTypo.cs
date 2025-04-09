using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixDeliveryMethodColumnsTypo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortNamr",
                table: "DeliveryMethods",
                newName: "ShortName");

            migrationBuilder.RenameColumn(
                name: "DeleviryTime",
                table: "DeliveryMethods",
                newName: "DeliveryTime");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShortName",
                table: "DeliveryMethods",
                newName: "ShortNamr");

            migrationBuilder.RenameColumn(
                name: "DeliveryTime",
                table: "DeliveryMethods",
                newName: "DeleviryTime");
        }
    }
}
