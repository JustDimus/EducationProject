using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationProject.EFCore.Migrations
{
    public partial class AddedUniquePhoneNumberToAccoun : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Accounts_PhoneNumber",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PhoneNumber",
                table: "Accounts",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Accounts_PhoneNumber",
                table: "Accounts");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Accounts_PhoneNumber",
                table: "Accounts",
                column: "PhoneNumber");
        }
    }
}
