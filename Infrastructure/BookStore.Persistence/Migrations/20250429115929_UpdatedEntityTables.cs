using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedEntityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "books");

            migrationBuilder.DropColumn(
                name: "DeletedTime",
                table: "authors");

            migrationBuilder.RenameColumn(
                name: "DeletedTime",
                table: "users",
                newName: "DeletedDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "categories",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "books",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "authors",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "books");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "authors");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "users",
                newName: "DeletedTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "categories",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "books",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedTime",
                table: "authors",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
