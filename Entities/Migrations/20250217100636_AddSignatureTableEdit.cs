using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddSignatureTableEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_signature",
                table: "signature");

            migrationBuilder.RenameTable(
                name: "signature",
                newName: "signatures");

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

            migrationBuilder.AddPrimaryKey(
                name: "pk_signatures",
                table: "signatures",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_signatures",
                table: "signatures");

            migrationBuilder.RenameTable(
                name: "signatures",
                newName: "signature");

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

            migrationBuilder.AddPrimaryKey(
                name: "pk_signature",
                table: "signature",
                column: "id");
        }
    }
}
