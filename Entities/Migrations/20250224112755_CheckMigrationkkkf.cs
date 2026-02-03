using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class CheckMigrationkkkf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "fk_sec_group_controls_sec_control_lists_control_codee",
            //    table: "sec_group_controls");

            //migrationBuilder.RenameColumn(
            //    name: "control_codee",
            //    table: "sec_group_controls",
            //    newName: "control_code");

            //migrationBuilder.RenameIndex(
            //    name: "ix_sec_group_controls_group_code_control_codee_page_code",
            //    table: "sec_group_controls",
            //    newName: "ix_sec_group_controls_group_code_control_code_page_code");

            //migrationBuilder.RenameIndex(
            //    name: "ix_sec_group_controls_control_codee",
            //    table: "sec_group_controls",
            //    newName: "ix_sec_group_controls_control_code");

            //migrationBuilder.AlterColumn<decimal>(
            //    name: "module_no",
            //    table: "audit_logs",
            //    type: "numeric(5)",
            //    precision: 5,
            //    nullable: true,
            //    oldClrType: typeof(decimal),
            //    oldType: "numeric(5,0)",
            //    oldPrecision: 5,
            //    oldNullable: true);

            //migrationBuilder.AddForeignKey(
            //    name: "fk_sec_group_controls_sec_control_lists_control_code",
            //    table: "sec_group_controls",
            //    column: "control_code",
            //    principalTable: "sec_control_lists",
            //    principalColumn: "id",
            //    onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_code",
                table: "sec_group_controls");

            migrationBuilder.RenameColumn(
                name: "control_code",
                table: "sec_group_controls",
                newName: "control_codee");

            migrationBuilder.RenameIndex(
                name: "ix_sec_group_controls_group_code_control_code_page_code",
                table: "sec_group_controls",
                newName: "ix_sec_group_controls_group_code_control_codee_page_code");

            migrationBuilder.RenameIndex(
                name: "ix_sec_group_controls_control_code",
                table: "sec_group_controls",
                newName: "ix_sec_group_controls_control_codee");

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

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_codee",
                table: "sec_group_controls",
                column: "control_codee",
                principalTable: "sec_control_lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
