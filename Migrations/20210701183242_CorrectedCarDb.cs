using Microsoft.EntityFrameworkCore.Migrations;

namespace CarShop.Migrations
{
    public partial class CorrectedCarDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BidCar");

            migrationBuilder.AddColumn<int>(
                name: "CarId",
                table: "Bids",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bids_CarId",
                table: "Bids",
                column: "CarId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bids_Cars_CarId",
                table: "Bids",
                column: "CarId",
                principalTable: "Cars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bids_Cars_CarId",
                table: "Bids");

            migrationBuilder.DropIndex(
                name: "IX_Bids_CarId",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "CarId",
                table: "Bids");

            migrationBuilder.CreateTable(
                name: "BidCar",
                columns: table => new
                {
                    BidsId = table.Column<int>(type: "int", nullable: false),
                    CarsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BidCar", x => new { x.BidsId, x.CarsId });
                    table.ForeignKey(
                        name: "FK_BidCar_Bids_BidsId",
                        column: x => x.BidsId,
                        principalTable: "Bids",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BidCar_Cars_CarsId",
                        column: x => x.CarsId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BidCar_CarsId",
                table: "BidCar",
                column: "CarsId");
        }
    }
}
