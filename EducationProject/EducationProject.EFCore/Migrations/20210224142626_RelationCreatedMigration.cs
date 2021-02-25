using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationProject.EFCore.Migrations
{
    public partial class RelationCreatedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCourses_Accounts_AccountID",
                table: "AccountCourses");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Accounts_PhoneNumber",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountID",
                table: "AccountCourses",
                newName: "AccountId");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Courses",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "PhoneNumber",
                table: "Accounts",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.CreateIndex(
                name: "IX_Courses_CreatorId",
                table: "Courses",
                column: "CreatorId");

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_PhoneNumber",
                table: "Accounts",
                column: "PhoneNumber",
                unique: true,
                filter: "[PhoneNumber] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCourses_Accounts_AccountId",
                table: "AccountCourses",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Accounts_CreatorId",
                table: "Courses",
                column: "CreatorId",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountCourses_Accounts_AccountId",
                table: "AccountCourses");

            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Accounts_CreatorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_CreatorId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_PhoneNumber",
                table: "Accounts");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "AccountCourses",
                newName: "AccountID");

            migrationBuilder.AlterColumn<int>(
                name: "CreatorId",
                table: "Courses",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_AccountCourses_Accounts_AccountID",
                table: "AccountCourses",
                column: "AccountID",
                principalTable: "Accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
