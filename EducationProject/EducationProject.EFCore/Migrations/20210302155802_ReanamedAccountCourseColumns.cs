using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationProject.EFCore.Migrations
{
    public partial class ReanamedAccountCourseColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMaterials_Courses_CourseID",
                table: "CourseMaterials");

            migrationBuilder.RenameColumn(
                name: "CourseID",
                table: "CourseMaterials",
                newName: "CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMaterials_Courses_CourseId",
                table: "CourseMaterials",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseMaterials_Courses_CourseId",
                table: "CourseMaterials");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "CourseMaterials",
                newName: "CourseID");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseMaterials_Courses_CourseID",
                table: "CourseMaterials",
                column: "CourseID",
                principalTable: "Courses",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
