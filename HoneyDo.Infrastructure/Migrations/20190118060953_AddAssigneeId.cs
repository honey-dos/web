using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneyDo.Infrastructure.Migrations
{
    public partial class AddAssigneeId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AssigneeId",
                table: "Todos",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssigneeId",
                table: "Todos");
        }
    }
}
