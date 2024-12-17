using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QBCustomer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerModelRelationships1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "qbAddresses",
                newName: "QuickBooksAddressId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "QuickBooksAddressId",
                table: "qbAddresses",
                newName: "Id");
        }
    }
}
