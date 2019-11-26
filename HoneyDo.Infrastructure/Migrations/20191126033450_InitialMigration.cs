using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace HoneyDo.Infrastructure.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    IsEnabled = table.Column<bool>(nullable: false),
                    Picture = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    NormalizedUserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "Logins",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    Provider = table.Column<string>(nullable: true),
                    ProviderKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logins", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupAccounts",
                columns: table => new
                {
                    GroupId = table.Column<Guid>(nullable: false),
                    AccountId = table.Column<Guid>(nullable: false),
                    GroupId1 = table.Column<Guid>(nullable: true),
                    AccountId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupAccounts", x => new { x.GroupId, x.AccountId });
                    table.ForeignKey(
                        name: "FK_GroupAccounts_Accounts_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAccounts_Accounts_AccountId1",
                        column: x => x.AccountId1,
                        principalTable: "Accounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GroupAccounts_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GroupAccounts_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Todos",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    CreatorId = table.Column<Guid>(nullable: false),
                    AssigneeId = table.Column<Guid>(nullable: true),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    CompletedDate = table.Column<DateTime>(nullable: true),
                    DueDate = table.Column<DateTime>(nullable: true),
                    GroupId = table.Column<Guid>(nullable: true),
                    GroupId1 = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Todos_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Todos_Groups_GroupId1",
                        column: x => x.GroupId1,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_NormalizedUserName",
                table: "Accounts",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccounts_AccountId",
                table: "GroupAccounts",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccounts_AccountId1",
                table: "GroupAccounts",
                column: "AccountId1");

            migrationBuilder.CreateIndex(
                name: "IX_GroupAccounts_GroupId1",
                table: "GroupAccounts",
                column: "GroupId1");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_GroupId",
                table: "Todos",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Todos_GroupId1",
                table: "Todos",
                column: "GroupId1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GroupAccounts");

            migrationBuilder.DropTable(
                name: "Logins");

            migrationBuilder.DropTable(
                name: "Todos");

            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
