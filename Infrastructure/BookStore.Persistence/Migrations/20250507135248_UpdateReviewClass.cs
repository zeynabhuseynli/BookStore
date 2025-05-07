using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookStore.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateReviewClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviews_ParentRewievId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_books_BookId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_users_FromUserId",
                table: "Reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ParentRewievId",
                table: "reviews",
                newName: "IX_reviews_ParentRewievId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_FromUserId_ToUserId",
                table: "reviews",
                newName: "IX_reviews_FromUserId_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BookId",
                table: "reviews",
                newName: "IX_reviews_BookId");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "users",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "reviews",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "reviews",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "categories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "categories",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "books",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "books",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "authors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "authors",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_reviews",
                table: "reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_books_BookId",
                table: "reviews",
                column: "BookId",
                principalTable: "books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_reviews_ParentRewievId",
                table: "reviews",
                column: "ParentRewievId",
                principalTable: "reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_reviews_users_FromUserId",
                table: "reviews",
                column: "FromUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_reviews_books_BookId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_reviews_ParentRewievId",
                table: "reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_reviews_users_FromUserId",
                table: "reviews");

            migrationBuilder.DropPrimaryKey(
                name: "PK_reviews",
                table: "reviews");

            migrationBuilder.RenameTable(
                name: "reviews",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_ParentRewievId",
                table: "Reviews",
                newName: "IX_Reviews_ParentRewievId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_FromUserId_ToUserId",
                table: "Reviews",
                newName: "IX_Reviews_FromUserId_ToUserId");

            migrationBuilder.RenameIndex(
                name: "IX_reviews_BookId",
                table: "Reviews",
                newName: "IX_Reviews_BookId");

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "users",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "Reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "categories",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "books",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "UpdatedById",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DeletedById",
                table: "authors",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviews_ParentRewievId",
                table: "Reviews",
                column: "ParentRewievId",
                principalTable: "Reviews",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_books_BookId",
                table: "Reviews",
                column: "BookId",
                principalTable: "books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_users_FromUserId",
                table: "Reviews",
                column: "FromUserId",
                principalTable: "users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
