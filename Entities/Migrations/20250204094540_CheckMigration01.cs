using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class CheckMigration01 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_module_group_sec_groups_group_code",
                table: "sec_module_group");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_module_group_sec_modules_module_code",
                table: "sec_module_group");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sec_module_group",
                table: "sec_module_group");

            migrationBuilder.RenameTable(
                name: "sec_module_group",
                newName: "sec_module_groups");

            migrationBuilder.RenameIndex(
                name: "ix_sec_module_group_module_code",
                table: "sec_module_groups",
                newName: "ix_sec_module_groups_module_code");

            migrationBuilder.RenameIndex(
                name: "ix_sec_module_group_group_code_module_code",
                table: "sec_module_groups",
                newName: "ix_sec_module_groups_group_code_module_code");


            migrationBuilder.AddPrimaryKey(
                name: "pk_sec_module_groups",
                table: "sec_module_groups",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_module_groups_sec_groups_group_code",
                table: "sec_module_groups",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_module_groups_sec_modules_module_code",
                table: "sec_module_groups",
                column: "module_code",
                principalTable: "sec_modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_module_groups_sec_groups_group_code",
                table: "sec_module_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_module_groups_sec_modules_module_code",
                table: "sec_module_groups");

            migrationBuilder.DropPrimaryKey(
                name: "pk_sec_module_groups",
                table: "sec_module_groups");

            migrationBuilder.RenameTable(
                name: "sec_module_groups",
                newName: "sec_module_group");

            migrationBuilder.RenameIndex(
                name: "ix_sec_module_groups_module_code",
                table: "sec_module_group",
                newName: "ix_sec_module_group_module_code");

            migrationBuilder.RenameIndex(
                name: "ix_sec_module_groups_group_code_module_code",
                table: "sec_module_group",
                newName: "ix_sec_module_group_group_code_module_code");

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
                name: "pk_sec_module_group",
                table: "sec_module_group",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_module_group_sec_groups_group_code",
                table: "sec_module_group",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_module_group_sec_modules_module_code",
                table: "sec_module_group",
                column: "module_code",
                principalTable: "sec_modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
