using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOwnerAndUserForeignKeys : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Tenants",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OwnerId",
                table: "Properties",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties",
                column: "OwnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Users_OwnerId",
                table: "Properties",
                column: "OwnerId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tenants_Users_UserId",
                table: "Tenants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Users_OwnerId",
                table: "Properties");

            migrationBuilder.DropForeignKey(
                name: "FK_Tenants_Users_UserId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Tenants_UserId",
                table: "Tenants");

            migrationBuilder.DropIndex(
                name: "IX_Properties_OwnerId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Tenants");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Properties");
        }
    }
}
