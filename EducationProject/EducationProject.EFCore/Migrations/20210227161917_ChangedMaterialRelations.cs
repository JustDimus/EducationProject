using Microsoft.EntityFrameworkCore.Migrations;

namespace EducationProject.EFCore.Migrations
{
    public partial class ChangedMaterialRelations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleMaterials_BaseMaterials_Id",
                table: "ArticleMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_BookMaterials_BaseMaterials_Id",
                table: "BookMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoMaterials_BaseMaterials_Id",
                table: "VideoMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleMaterials_BaseMaterials_Id",
                table: "ArticleMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookMaterials_BaseMaterials_Id",
                table: "BookMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoMaterials_BaseMaterials_Id",
                table: "VideoMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ArticleMaterials_BaseMaterials_Id",
                table: "ArticleMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_BookMaterials_BaseMaterials_Id",
                table: "BookMaterials");

            migrationBuilder.DropForeignKey(
                name: "FK_VideoMaterials_BaseMaterials_Id",
                table: "VideoMaterials");

            migrationBuilder.AddForeignKey(
                name: "FK_ArticleMaterials_BaseMaterials_Id",
                table: "ArticleMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BookMaterials_BaseMaterials_Id",
                table: "BookMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_VideoMaterials_BaseMaterials_Id",
                table: "VideoMaterials",
                column: "Id",
                principalTable: "BaseMaterials",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
