using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Motorcycle_Rental_Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Updateconfigurationtables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoleEnterprisesTenants_AspNetRoles_RoleId",
                table: "ApplicationUserRoleEnterprisesTenants");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoleEnterprisesTenants_AspNetUsers_UserId",
                table: "ApplicationUserRoleEnterprisesTenants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserRoleEnterprisesTenants",
                table: "ApplicationUserRoleEnterprisesTenants");

            migrationBuilder.RenameTable(
                name: "ApplicationUserRoleEnterprisesTenants",
                newName: "ApplicationUserRoles");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserRoleEnterprisesTenants_UserId",
                table: "ApplicationUserRoles",
                newName: "IX_ApplicationUserRoles_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserRoleEnterprisesTenants_RoleId",
                table: "ApplicationUserRoles",
                newName: "IX_ApplicationUserRoles_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserRoles",
                table: "ApplicationUserRoles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_AspNetRoles_RoleId",
                table: "ApplicationUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoles_AspNetUsers_UserId",
                table: "ApplicationUserRoles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_AspNetRoles_RoleId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationUserRoles_AspNetUsers_UserId",
                table: "ApplicationUserRoles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ApplicationUserRoles",
                table: "ApplicationUserRoles");

            migrationBuilder.RenameTable(
                name: "ApplicationUserRoles",
                newName: "ApplicationUserRoleEnterprisesTenants");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserRoles_UserId",
                table: "ApplicationUserRoleEnterprisesTenants",
                newName: "IX_ApplicationUserRoleEnterprisesTenants_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ApplicationUserRoles_RoleId",
                table: "ApplicationUserRoleEnterprisesTenants",
                newName: "IX_ApplicationUserRoleEnterprisesTenants_RoleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ApplicationUserRoleEnterprisesTenants",
                table: "ApplicationUserRoleEnterprisesTenants",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoleEnterprisesTenants_AspNetRoles_RoleId",
                table: "ApplicationUserRoleEnterprisesTenants",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationUserRoleEnterprisesTenants_AspNetUsers_UserId",
                table: "ApplicationUserRoleEnterprisesTenants",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
