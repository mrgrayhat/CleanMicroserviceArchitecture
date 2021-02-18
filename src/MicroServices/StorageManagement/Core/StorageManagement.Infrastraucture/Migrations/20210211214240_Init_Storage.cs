using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace StorageManagement.Infrastraucture.Migrations
{
    public partial class Init_Storage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: true),
                    Url = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true),
                    Description = table.Column<string>(type: "TEXT", maxLength: 250, nullable: true),
                    Size = table.Column<long>(type: "INTEGER", nullable: false),
                    ContentType = table.Column<string>(type: "TEXT", maxLength: 25, nullable: true),
                    Downloaded = table.Column<long>(type: "INTEGER", nullable: false),
                    VerifiedHash = table.Column<string>(type: "TEXT", maxLength: 32, nullable: true),
                    Tags = table.Column<string>(type: "TEXT", maxLength: 254, nullable: true),
                    CreatedBy = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "TEXT", maxLength: 16, nullable: true),
                    LastModified = table.Column<DateTime>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Items_Id",
                table: "Items",
                column: "Id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");
        }
    }
}
