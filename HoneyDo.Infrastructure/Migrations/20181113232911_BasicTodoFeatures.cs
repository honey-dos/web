using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneyDo.Infrastructure.Migrations
{
    public partial class BasicTodoFeatures : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CompletedDate",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "Todos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "Todos",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Todos",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompletedDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "Todos");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Todos");
        }
    }
}
