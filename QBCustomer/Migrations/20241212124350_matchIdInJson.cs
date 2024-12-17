using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QBCustomer.Migrations
{
    /// <inheritdoc />
    public partial class matchIdInJson : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdressId",
                table: "qbAddresses",
                newName: "AddressId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "qbAddresses",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AddressId",
                table: "qbAddresses",
                newName: "AdressId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "qbAddresses",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
