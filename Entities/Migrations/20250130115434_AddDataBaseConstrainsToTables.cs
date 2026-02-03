using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddDataBaseConstrainsToTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_control_lists_sec_pages_page_id",
                table: "sec_control_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_code",
                table: "sec_group_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_groups_group_code",
                table: "sec_group_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_jobs_sec_groups_group_code",
                table: "sec_group_jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_groups_group_code",
                table: "sec_group_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_groups_sec_modules_module_no",
                table: "sec_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_services_sec_modules_module_no",
                table: "sec_services");

            migrationBuilder.DropIndex(
                name: "ix_sec_groups_module_no",
                table: "sec_groups");

            migrationBuilder.DropColumn(
                name: "module_no",
                table: "sec_pages");

            migrationBuilder.DropColumn(
                name: "page_name_a",
                table: "sec_pages");

            migrationBuilder.DropColumn(
                name: "parent_page_id",
                table: "sec_pages");

            migrationBuilder.DropColumn(
                name: "module_no",
                table: "sec_groups");

            migrationBuilder.RenameColumn(
                name: "service_no",
                table: "sec_pages",
                newName: "service_code");

            migrationBuilder.AddColumn<int>(
                name: "module_code",
                table: "sec_pages",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                comment: "Module number associated with the page");

            migrationBuilder.AddColumn<int>(
                name: "parent_id",
                table: "sec_pages",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "sec_module_id",
                table: "sec_groups",
                type: "integer",
                nullable: true);

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

            migrationBuilder.CreateTable(
                name: "sec_module_group",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_code = table.Column<int>(type: "integer", nullable: false),
                    group_code = table.Column<int>(type: "integer", nullable: false),
                    insert_user_code = table.Column<string>(type: "text", nullable: true),
                    insert_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    update_user_code = table.Column<string>(type: "text", nullable: true),
                    last_update = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    is_deleted = table.Column<bool>(type: "boolean", nullable: false),
                    delete_user_code = table.Column<string>(type: "text", nullable: true),
                    delete_date = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_sec_module_group", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_module_group_sec_groups_group_code",
                        column: x => x.group_code,
                        principalTable: "sec_groups",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "fk_sec_module_group_sec_modules_module_code",
                        column: x => x.module_code,
                        principalTable: "sec_modules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_sec_pages_module_code",
                table: "sec_pages",
                column: "module_code");

            migrationBuilder.CreateIndex(
                name: "ix_sec_pages_parent_id",
                table: "sec_pages",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_pages_service_code",
                table: "sec_pages",
                column: "service_code");

            migrationBuilder.CreateIndex(
                name: "ix_sec_groups_sec_module_id",
                table: "sec_groups",
                column: "sec_module_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_module_group_group_code",
                table: "sec_module_group",
                column: "group_code");

            migrationBuilder.CreateIndex(
                name: "ix_sec_module_group_module_code",
                table: "sec_module_group",
                column: "module_code");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_control_lists_sec_pages_page_id",
                table: "sec_control_lists",
                column: "page_id",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_code",
                table: "sec_group_controls",
                column: "control_code",
                principalTable: "sec_control_lists",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_groups_group_code",
                table: "sec_group_controls",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_employees_sec_groups_group_code",
                table: "sec_group_employees",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_jobs_sec_groups_group_code",
                table: "sec_group_jobs",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_groups_group_code",
                table: "sec_group_pages",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages",
                column: "page_id",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_groups_sec_modules_sec_module_id",
                table: "sec_groups",
                column: "sec_module_id",
                principalTable: "sec_modules",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_pages_sec_modules_module_code",
                table: "sec_pages",
                column: "module_code",
                principalTable: "sec_modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_pages_sec_pages_parent_id",
                table: "sec_pages",
                column: "parent_id",
                principalTable: "sec_pages",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_pages_sec_services_service_code",
                table: "sec_pages",
                column: "service_code",
                principalTable: "sec_services",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_sec_services_sec_modules_module_no",
                table: "sec_services",
                column: "module_no",
                principalTable: "sec_modules",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_sec_control_lists_sec_pages_page_id",
                table: "sec_control_lists");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_code",
                table: "sec_group_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_controls_sec_groups_group_code",
                table: "sec_group_controls");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_employees_sec_groups_group_code",
                table: "sec_group_employees");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_jobs_sec_groups_group_code",
                table: "sec_group_jobs");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_groups_group_code",
                table: "sec_group_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_groups_sec_modules_sec_module_id",
                table: "sec_groups");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_pages_sec_modules_module_code",
                table: "sec_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_pages_sec_pages_parent_id",
                table: "sec_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_pages_sec_services_service_code",
                table: "sec_pages");

            migrationBuilder.DropForeignKey(
                name: "fk_sec_services_sec_modules_module_no",
                table: "sec_services");

            migrationBuilder.DropTable(
                name: "sec_module_group");

            migrationBuilder.DropIndex(
                name: "ix_sec_pages_module_code",
                table: "sec_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_pages_parent_id",
                table: "sec_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_pages_service_code",
                table: "sec_pages");

            migrationBuilder.DropIndex(
                name: "ix_sec_groups_sec_module_id",
                table: "sec_groups");

            migrationBuilder.DropColumn(
                name: "module_code",
                table: "sec_pages");

            migrationBuilder.DropColumn(
                name: "parent_id",
                table: "sec_pages");

            migrationBuilder.DropColumn(
                name: "sec_module_id",
                table: "sec_groups");

            migrationBuilder.RenameColumn(
                name: "service_code",
                table: "sec_pages",
                newName: "service_no");

            migrationBuilder.AddColumn<int>(
                name: "module_no",
                table: "sec_pages",
                type: "integer",
                nullable: true,
                comment: "Module number associated with the page");

            migrationBuilder.AddColumn<string>(
                name: "page_name_a",
                table: "sec_pages",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true,
                comment: "Page name in Arabic");

            migrationBuilder.AddColumn<string>(
                name: "parent_page_id",
                table: "sec_pages",
                type: "character varying(15)",
                maxLength: 15,
                nullable: true,
                comment: "Parent page identifier");

            migrationBuilder.AddColumn<int>(
                name: "module_no",
                table: "sec_groups",
                type: "integer",
                nullable: true,
                comment: "Foreign key referencing SEC_MODULES table");

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
                name: "ix_sec_groups_module_no",
                table: "sec_groups",
                column: "module_no");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_control_lists_sec_pages_page_id",
                table: "sec_control_lists",
                column: "page_id",
                principalTable: "sec_pages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_control_lists_control_code",
                table: "sec_group_controls",
                column: "control_code",
                principalTable: "sec_control_lists",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_controls_sec_groups_group_code",
                table: "sec_group_controls",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_jobs_sec_groups_group_code",
                table: "sec_group_jobs",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_groups_group_code",
                table: "sec_group_pages",
                column: "group_code",
                principalTable: "sec_groups",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_group_pages_sec_pages_page_id",
                table: "sec_group_pages",
                column: "page_id",
                principalTable: "sec_pages",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_groups_sec_modules_module_no",
                table: "sec_groups",
                column: "module_no",
                principalTable: "sec_modules",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_sec_services_sec_modules_module_no",
                table: "sec_services",
                column: "module_no",
                principalTable: "sec_modules",
                principalColumn: "id");
        }
    }
}
