using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateBaseEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "users",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "Reviews",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "categories",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "books",
                newName: "DeletedAt");

            migrationBuilder.RenameColumn(
                name: "DeletedDate",
                table: "authors",
                newName: "DeletedAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "users",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldDefaultValueSql: "CURRENT_TIMESTAMP");

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "Reviews",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CreatedById",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "DeletedById",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UpdatedById",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "users");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "users");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "users");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "books");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "books");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "books");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "DeletedById",
                table: "authors");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "authors");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "users",
                newName: "DeletedDate");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "Reviews",
                newName: "DeletedDate");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "categories",
                newName: "DeletedDate");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "books",
                newName: "DeletedDate");

            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "authors",
                newName: "DeletedDate");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "users",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.AlterColumn<bool>(
                name: "IsDeleted",
                table: "Reviews",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedAt",
                table: "Reviews",
                type: "timestamp with time zone",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP",
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");
        }
    }
}
