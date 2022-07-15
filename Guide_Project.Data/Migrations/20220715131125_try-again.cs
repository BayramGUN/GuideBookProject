using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Guide_Project.Data.Migrations
{
    public partial class tryagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Customers_CommercialActivities_CommercialActivitiesId",
                table: "Customers");

            migrationBuilder.DropIndex(
                name: "IX_Customers_CommercialActivitiesId",
                table: "Customers");

            migrationBuilder.DropColumn(
                name: "CommercialActivitiesId",
                table: "Customers");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Customers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Customers");

            migrationBuilder.AddColumn<int>(
                name: "CommercialActivitiesId",
                table: "Customers",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Customers_CommercialActivitiesId",
                table: "Customers",
                column: "CommercialActivitiesId");

            migrationBuilder.AddForeignKey(
                name: "FK_Customers_CommercialActivities_CommercialActivitiesId",
                table: "Customers",
                column: "CommercialActivitiesId",
                principalTable: "CommercialActivities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
