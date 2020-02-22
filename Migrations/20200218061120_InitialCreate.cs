using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProjectManagement.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "Companies",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 30, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Companies", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Employees",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Surname = table.Column<string>(maxLength: 30, nullable: false),
            //        Name = table.Column<string>(maxLength: 15, nullable: false),
            //        Patronymic = table.Column<string>(maxLength: 15, nullable: true),
            //        Email = table.Column<string>(nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Employees", x => x.Id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "Projects",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        Name = table.Column<string>(maxLength: 30, nullable: false),
            //        StartDate = table.Column<DateTime>(nullable: false),
            //        FinishDate = table.Column<DateTime>(nullable: false),
            //        Priority = table.Column<int>(nullable: false),
            //        ManagerId = table.Column<int>(nullable: true),
            //        CustomerId = table.Column<int>(nullable: true),
            //        PerformerId = table.Column<int>(nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_Projects", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_Projects_Companies_CustomerId",
            //            column: x => x.CustomerId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Projects_Employees_ManagerId",
            //            column: x => x.ManagerId,
            //            principalTable: "Employees",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_Projects_Companies_PerformerId",
            //            column: x => x.PerformerId,
            //            principalTable: "Companies",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "IX_Projects_CustomerId",
            //    table: "Projects",
            //    column: "CustomerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Projects_ManagerId",
            //    table: "Projects",
            //    column: "ManagerId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Projects_PerformerId",
            //    table: "Projects",
            //    column: "PerformerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Companies");

            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
