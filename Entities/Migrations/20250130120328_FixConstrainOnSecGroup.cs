using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class FixConstrainOnSecGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_groups_sec_modules_sec_module_id",
                table: "sec_groups");

            migrationBuilder.DropIndex(
                name: "ix_sec_groups_sec_module_id",
                table: "sec_groups");

            migrationBuilder.DropColumn(
                name: "sec_module_id",
                table: "sec_groups");

            migrationBuilder.AlterColumn<decimal>(
                name: "module_no",
                table: "audit_logs",
                type: "numeric(5)",
                precision: 5,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,0)",
                oldPrecision: 5,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "sec_module_id",
                table: "sec_groups",
                type: "integer",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "module_no",
                table: "audit_logs",
                type: "numeric(5,0)",
                precision: 5,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5)",
                oldPrecision: 5,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_groups_sec_module_id",
                table: "sec_groups",
                column: "sec_module_id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_groups_sec_modules_sec_module_id",
                table: "sec_groups",
                column: "sec_module_id",
                principalTable: "sec_modules",
                principalColumn: "id");
        }
    }
}
