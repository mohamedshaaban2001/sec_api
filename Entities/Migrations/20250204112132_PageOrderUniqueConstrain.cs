using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class PageOrderUniqueConstrain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "ix_sec_pages_page_order",
                table: "sec_pages",
                column: "page_order",
                unique: true,
                filter: "is_deleted = FALSE");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_sec_pages_page_order",
                table: "sec_pages");

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
        }
    }
}
