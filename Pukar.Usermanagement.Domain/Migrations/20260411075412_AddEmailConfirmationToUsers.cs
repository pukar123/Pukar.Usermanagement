using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Pukar.Usermanagement.Domain.Migrations
{
    /// <inheritdoc />
    public partial class AddEmailConfirmationToUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationExpiresAtUtc",
                schema: "um",
                table: "Users",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationTokenHash",
                schema: "um",
                table: "Users",
                type: "nvarchar(128)",
                maxLength: 128,
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EmailConfirmed",
                schema: "um",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            // Existing accounts created before email confirmation should remain able to sign in.
            migrationBuilder.Sql("UPDATE [um].[Users] SET [EmailConfirmed] = 1");

            migrationBuilder.CreateIndex(
                name: "IX_Users_EmailConfirmationTokenHash",
                schema: "um",
                table: "Users",
                column: "EmailConfirmationTokenHash",
                unique: true,
                filter: "[EmailConfirmationTokenHash] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Users_EmailConfirmationTokenHash",
                schema: "um",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationExpiresAtUtc",
                schema: "um",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationTokenHash",
                schema: "um",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "EmailConfirmed",
                schema: "um",
                table: "Users");
        }
    }
}
