using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddColorAndIconForGroup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "unit_code",
                table: "sec_groups");

            migrationBuilder.AddColumn<string>(
                name: "color",
                table: "sec_groups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Color of the group");

            migrationBuilder.AddColumn<string>(
                name: "icon",
                table: "sec_groups",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                comment: "Icon of the group");

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
            migrationBuilder.DropColumn(
                name: "color",
                table: "sec_groups");

            migrationBuilder.DropColumn(
                name: "icon",
                table: "sec_groups");

            migrationBuilder.AddColumn<string>(
                name: "unit_code",
                table: "sec_groups",
                type: "character varying(10)",
                maxLength: 10,
                nullable: true,
                comment: "Unit code associated with the group");

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
