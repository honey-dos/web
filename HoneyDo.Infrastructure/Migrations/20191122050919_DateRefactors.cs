using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneyDo.Infrastructure.Migrations
{
    public partial class DateRefactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerId",
                table: "Todos",
                newName: "CreatorId");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Todos",
                newName: "DateModified");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "Todos",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "Todos");

            migrationBuilder.RenameColumn(
                name: "DateModified",
                table: "Todos",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatorId",
                table: "Todos",
                newName: "OwnerId");
        }
    }
}
