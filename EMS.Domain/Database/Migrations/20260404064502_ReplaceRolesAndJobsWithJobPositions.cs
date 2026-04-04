using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EMS.Domain.Database.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceRolesAndJobsWithJobPositions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Jobs_JobId",
                schema: "org",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "Jobs",
                schema: "org");

            migrationBuilder.DropTable(
                name: "Roles",
                schema: "org");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobId",
                schema: "org",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "JobId",
                schema: "org",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "JobPositionId",
                schema: "org",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "JobPositions",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JobPositions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_JobPositions_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "org",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_OrganizationId_Code",
                schema: "org",
                table: "JobPositions",
                columns: new[] { "OrganizationId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_JobPositions_OrganizationId_Title",
                schema: "org",
                table: "JobPositions",
                columns: new[] { "OrganizationId", "Title" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_JobPositions_JobPositionId",
                schema: "org",
                table: "Employees",
                column: "JobPositionId",
                principalSchema: "org",
                principalTable: "JobPositions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobPositionId",
                schema: "org",
                table: "Employees",
                column: "JobPositionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_JobPositions_JobPositionId",
                schema: "org",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_JobPositionId",
                schema: "org",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "JobPositions",
                schema: "org");

            migrationBuilder.DropColumn(
                name: "JobPositionId",
                schema: "org",
                table: "Employees");

            migrationBuilder.AddColumn<int>(
                name: "JobId",
                schema: "org",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_JobId",
                schema: "org",
                table: "Employees",
                column: "JobId");

            migrationBuilder.CreateTable(
                name: "Roles",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrganizationId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Roles_Organizations_OrganizationId",
                        column: x => x.OrganizationId,
                        principalSchema: "org",
                        principalTable: "Organizations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Jobs",
                schema: "org",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Jobs_Roles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "org",
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RoleId_Code",
                schema: "org",
                table: "Jobs",
                columns: new[] { "RoleId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_RoleId_Name",
                schema: "org",
                table: "Jobs",
                columns: new[] { "RoleId", "Name" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrganizationId_Code",
                schema: "org",
                table: "Roles",
                columns: new[] { "OrganizationId", "Code" },
                unique: true,
                filter: "[Code] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Roles_OrganizationId_Name",
                schema: "org",
                table: "Roles",
                columns: new[] { "OrganizationId", "Name" },
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Jobs_JobId",
                schema: "org",
                table: "Employees",
                column: "JobId",
                principalSchema: "org",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
