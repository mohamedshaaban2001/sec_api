using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class PageRequiredConstrains : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "page_order",
                table: "sec_pages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Order of the page",
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true,
                oldComment: "Order of the page");

            migrationBuilder.AlterColumn<string>(
                name: "page_name",
                table: "sec_pages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                comment: "Page name in English",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldNullable: true,
                oldComment: "Page name in English");

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
            migrationBuilder.AlterColumn<int>(
                name: "page_order",
                table: "sec_pages",
                type: "integer",
                nullable: true,
                comment: "Order of the page",
                oldClrType: typeof(int),
                oldType: "integer",
                oldComment: "Order of the page");

            migrationBuilder.AlterColumn<string>(
                name: "page_name",
                table: "sec_pages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true,
                comment: "Page name in English",
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100,
                oldComment: "Page name in English");

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
