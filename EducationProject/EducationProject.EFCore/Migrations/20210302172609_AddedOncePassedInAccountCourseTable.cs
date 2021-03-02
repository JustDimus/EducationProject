using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationProject.EFCore.Migrations
{
    public partial class AddedOncePassedInAccountCourseTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OncePassed",
                table: "AccountCourses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OncePassed",
                table: "AccountCourses");
        }
    }
}
