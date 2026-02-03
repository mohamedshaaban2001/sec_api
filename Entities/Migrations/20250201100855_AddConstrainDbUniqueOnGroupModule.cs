using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddConstrainDbUniqueOnGroupModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sec_module_group_group_code",
                table: "sec_module_group");

            migrationBuilder.AddColumn<int>(
                name: "page_code",
                table: "sec_group_controls",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.CreateIndex(
                name: "ix_sec_module_group_group_code_module_code",
                table: "sec_module_group",
                columns: new[] { "group_code", "module_code" },
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_page_code",
                table: "sec_group_controls",
                column: "page_code");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_pages_page_code",
                table: "sec_group_controls",
                column: "page_code",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_pages_page_code",
                table: "sec_group_controls");

            migrationBuilder.DropIndex(
                name: "ix_sec_module_group_group_code_module_code",
                table: "sec_module_group");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_controls_page_code",
                table: "sec_group_controls");

            migrationBuilder.DropColumn(
                name: "page_code",
                table: "sec_group_controls");

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
                name: "ix_sec_module_group_group_code",
                table: "sec_module_group",
                column: "group_code");
        }
    }
}
