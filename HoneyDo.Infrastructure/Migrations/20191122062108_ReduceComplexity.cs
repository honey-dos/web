using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneyDo.Infrastructure.Migrations
{
    public partial class ReduceComplexity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Groups_GroupId",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_GroupId",
                table: "Todos");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId1",
                table: "Todos",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Todos_GroupId1",
                table: "Todos",
                column: "GroupId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Groups_GroupId1",
                table: "Todos",
                column: "GroupId1",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Todos_Groups_GroupId1",
                table: "Todos");

            migrationBuilder.DropIndex(
                name: "IX_Todos_GroupId1",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "GroupId1",
                table: "Todos");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_GroupId",
                table: "Todos",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Todos_Groups_GroupId",
                table: "Todos",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
