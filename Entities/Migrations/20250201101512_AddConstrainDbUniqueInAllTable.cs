using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddConstrainDbUniqueInAllTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_pages_group_code_page_id",
                table: "sec_group_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_jobs_group_code_job_code",
                table: "sec_group_jobs");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_employees_group_code_emp_code",
                table: "sec_group_employees");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_controls_control_code_group_code",
                table: "sec_group_controls");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_controls_group_code",
                table: "sec_group_controls");

            migrationBuilder.RenameColumn(
                name: "page_id",
                table: "sec_group_pages",
                newName: "page_code");

            migrationBuilder.RenameIndex(
                name: "ix_sec_group_pages_page_id",
                table: "sec_group_pages",
                newName: "ix_sec_group_pages_page_code");

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
                name: "ix_sec_group_pages_group_code_page_code",
                table: "sec_group_pages",
                columns: new[] { "group_code", "page_code" },
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_jobs_group_code_job_code",
                table: "sec_group_jobs",
                columns: new[] { "group_code", "job_code" },
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_employees_group_code_emp_code",
                table: "sec_group_employees",
                columns: new[] { "group_code", "emp_code" },
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_control_code",
                table: "sec_group_controls",
                column: "control_code");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_group_code_control_code_page_code",
                table: "sec_group_controls",
                columns: new[] { "group_code", "control_code", "page_code" },
                unique: true,
                filter: "is_deleted = FALSE");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_code",
                table: "sec_group_pages",
                column: "page_code",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_code",
                table: "sec_group_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_pages_group_code_page_code",
                table: "sec_group_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_jobs_group_code_job_code",
                table: "sec_group_jobs");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_employees_group_code_emp_code",
                table: "sec_group_employees");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_controls_control_code",
                table: "sec_group_controls");

            migrationBuilder.DropIndex(
                name: "ix_sec_group_controls_group_code_control_code_page_code",
                table: "sec_group_controls");

            migrationBuilder.RenameColumn(
                name: "page_code",
                table: "sec_group_pages",
                newName: "page_id");

            migrationBuilder.RenameIndex(
                name: "ix_sec_group_pages_page_code",
                table: "sec_group_pages",
                newName: "ix_sec_group_pages_page_id");

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
                name: "ix_sec_group_pages_group_code_page_id",
                table: "sec_group_pages",
                columns: new[] { "group_code", "page_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_jobs_group_code_job_code",
                table: "sec_group_jobs",
                columns: new[] { "group_code", "job_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_employees_group_code_emp_code",
                table: "sec_group_employees",
                columns: new[] { "group_code", "emp_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_control_code_group_code",
                table: "sec_group_controls",
                columns: new[] { "control_code", "group_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_group_code",
                table: "sec_group_controls",
                column: "group_code");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages",
                column: "page_id",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
