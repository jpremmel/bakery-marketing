using Microsoft.EntityFrameworkCore.Migrations;

namespace Bakery.Migrations
{
    public partial class UpdateTreatFlavorProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatFlavors_Flavors_FlavorId",
                table: "TreatFlavors");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "TreatFlavors");

            migrationBuilder.AlterColumn<int>(
                name: "FlavorId",
                table: "TreatFlavors",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatFlavors_Flavors_FlavorId",
                table: "TreatFlavors",
                column: "FlavorId",
                principalTable: "Flavors",
                principalColumn: "FlavorId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TreatFlavors_Flavors_FlavorId",
                table: "TreatFlavors");

            migrationBuilder.AlterColumn<int>(
                name: "FlavorId",
                table: "TreatFlavors",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "TreatFlavors",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_TreatFlavors_Flavors_FlavorId",
                table: "TreatFlavors",
                column: "FlavorId",
                principalTable: "Flavors",
                principalColumn: "FlavorId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
