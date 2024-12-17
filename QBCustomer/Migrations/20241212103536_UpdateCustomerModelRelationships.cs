using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QBCustomer.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCustomerModelRelationships : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "qbAddresses",
                columns: table => new
                {
                    AdressId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Line1 = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CountrySubDivisionCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Long = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Id = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_qbAddresses", x => x.AdressId);
                });

            migrationBuilder.CreateTable(
                name: "QbContactInfos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FreeFormNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QbContactInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Customers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    QuickBooksId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FullyQualifiedName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillWithParent = table.Column<bool>(type: "bit", nullable: true),
                    Job = table.Column<bool>(type: "bit", nullable: true),
                    Active = table.Column<bool>(type: "bit", nullable: true),
                    Taxable = table.Column<bool>(type: "bit", nullable: true),
                    Balance = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    BalanceWithJobs = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PreferredDeliveryMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrintOnCheckName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SyncToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BillAddrId = table.Column<int>(type: "int", nullable: true),
                    PrimaryPhoneId = table.Column<int>(type: "int", nullable: true),
                    PrimaryEmailAddrId = table.Column<int>(type: "int", nullable: true),
                    CreateTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastUpdatedTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Customers_QbContactInfos_PrimaryEmailAddrId",
                        column: x => x.PrimaryEmailAddrId,
                        principalTable: "QbContactInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_QbContactInfos_PrimaryPhoneId",
                        column: x => x.PrimaryPhoneId,
                        principalTable: "QbContactInfos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Customers_qbAddresses_BillAddrId",
                        column: x => x.BillAddrId,
                        principalTable: "qbAddresses",
                        principalColumn: "AdressId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Customers_BillAddrId",
                table: "Customers",
                column: "BillAddrId",
                unique: true,
                filter: "[BillAddrId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PrimaryEmailAddrId",
                table: "Customers",
                column: "PrimaryEmailAddrId",
                unique: true,
                filter: "[PrimaryEmailAddrId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Customers_PrimaryPhoneId",
                table: "Customers",
                column: "PrimaryPhoneId",
                unique: true,
                filter: "[PrimaryPhoneId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Customers");

            migrationBuilder.DropTable(
                name: "QbContactInfos");

            migrationBuilder.DropTable(
                name: "qbAddresses");
        }
    }
}
