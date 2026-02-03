using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class BaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    table_id = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    time_stamp = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    emp_serial = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true),
                    table_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    changes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    module_no = table.Column<decimal>(type: "numeric(5)", precision: 5, nullable: true),
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
                    table.PrimaryKey("pk_audit_logs", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "base_structures",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
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
                    table.PrimaryKey("pk_base_structures", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "sec_group_employees",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_code = table.Column<int>(type: "integer", nullable: false, comment: "Group code referencing a group"),
                    emp_code = table.Column<int>(type: "integer", nullable: false, comment: "Employee code referencing an employee"),
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
                    table.PrimaryKey("pk_sec_group_employees", x => x.id);
                },
                comment: "SEC_GROUPS_EMPLOYEES table for linking groups and employees");

            migrationBuilder.CreateTable(
                name: "sec_modules",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name of the module"),
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
                    table.PrimaryKey("pk_sec_modules", x => x.id);
                },
                comment: "SEC_MODULES table for storing module information");

            migrationBuilder.CreateTable(
                name: "sec_pages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    page_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true, comment: "Page name in English"),
                    page_name_a = table.Column<string>(type: "character varying(120)", maxLength: 120, nullable: true, comment: "Page name in Arabic"),
                    parent_page_id = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true, comment: "Parent page identifier"),
                    page_order = table.Column<int>(type: "integer", nullable: true, comment: "Order of the page"),
                    module_no = table.Column<int>(type: "integer", nullable: true, comment: "Module number associated with the page"),
                    icon = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Icon associated with the page"),
                    service_no = table.Column<int>(type: "integer", nullable: true),
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
                    table.PrimaryKey("pk_sec_pages", x => x.id);
                },
                comment: "SEC_PAGES table for storing page details");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, comment: "Name of the user"),
                    user_password = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Encrypted password for the user"),
                    user_desc = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Description of the user"),
                    person_code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true, comment: "Code associated with the person"),
                    memo = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true, comment: "Additional notes or memo for the user"),
                    mtg_person_role = table.Column<int>(type: "integer", nullable: true, comment: "Role of the person in meetings"),
                    mtg_rel_prsn = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true, comment: "Related person information for meetings"),
                    user_activation = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "1", comment: "Activation status of the user (default: 1)"),
                    version_no = table.Column<int>(type: "integer", nullable: true, comment: "Version number for the user"),
                    sign = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true, comment: "Digital signature of the user"),
                    signotech = table.Column<int>(type: "integer", nullable: true, defaultValueSql: "0", comment: "Flag for Signotec (default: 0)"),
                    emp_serial = table.Column<int>(type: "integer", nullable: true, comment: "Employee serial number associated with the user"),
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
                    table.PrimaryKey("pk_users", x => x.id);
                },
                comment: "USERS table for storing user information");

            migrationBuilder.CreateTable(
                name: "sec_groups",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name of the group"),
                    unit_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true, comment: "Unit code associated with the group"),
                    module_no = table.Column<int>(type: "integer", nullable: true, comment: "Foreign key referencing SEC_MODULES table"),
                    archive_role = table.Column<int>(type: "integer", nullable: true, comment: "Role for archive permissions"),
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
                    table.PrimaryKey("pk_sec_groups", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_groups_sec_modules_module_no",
                        column: x => x.module_no,
                        principalTable: "sec_modules",
                        principalColumn: "id");
                },
                comment: "SEC_GROUPS table for storing group information");

            migrationBuilder.CreateTable(
                name: "sec_services",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    module_no = table.Column<int>(type: "integer", nullable: true),
                    service_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name of the SERVICES "),
                    is_taken = table.Column<bool>(type: "boolean", nullable: false),
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
                    table.PrimaryKey("pk_sec_services", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_services_sec_modules_module_no",
                        column: x => x.module_no,
                        principalTable: "sec_modules",
                        principalColumn: "id");
                },
                comment: "SEC_SERVICES table for storing SERVICES information");

            migrationBuilder.CreateTable(
                name: "sec_control_lists",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    control_code = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Name of the control"),
                    control_description = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false, comment: "Description of the control"),
                    page_id = table.Column<int>(type: "integer", nullable: false, comment: "Foreign key referencing SEC_PAGES table"),
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
                    table.PrimaryKey("pk_sec_control_lists", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_control_lists_sec_pages_page_id",
                        column: x => x.page_id,
                        principalTable: "sec_pages",
                        principalColumn: "id");
                },
                comment: "SEC_CONTROLS_LIST table for storing control information");

            migrationBuilder.CreateTable(
                name: "sec_group_jobs",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_code = table.Column<int>(type: "integer", nullable: false, comment: "Group code referencing a group"),
                    job_code = table.Column<int>(type: "integer", nullable: false, comment: "Job code referencing a job"),
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
                    table.PrimaryKey("pk_sec_group_jobs", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_group_jobs_sec_groups_group_code",
                        column: x => x.group_code,
                        principalTable: "sec_groups",
                        principalColumn: "id");
                },
                comment: "SEC_GROUPS_JOBS table for linking groups and jobs");

            migrationBuilder.CreateTable(
                name: "sec_group_pages",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    group_code = table.Column<int>(type: "integer", nullable: false, comment: "Group code referencing a group"),
                    page_id = table.Column<int>(type: "integer", nullable: false, comment: "Page ID referencing a page"),
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
                    table.PrimaryKey("pk_sec_group_pages", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_group_pages_sec_groups_group_code",
                        column: x => x.group_code,
                        principalTable: "sec_groups",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sec_group_pages_sec_pages_page_id",
                        column: x => x.page_id,
                        principalTable: "sec_pages",
                        principalColumn: "id");
                },
                comment: "SEC_GROUPS_PAGES table for linking groups and pages");

            migrationBuilder.CreateTable(
                name: "sec_group_controls",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    control_code = table.Column<int>(type: "integer", nullable: false, comment: "Control code, foreign key referencing SEC_CONTROLS_LIST"),
                    group_code = table.Column<int>(type: "integer", nullable: false, comment: "Group code, foreign key referencing SEC_GROUPS"),
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
                    table.PrimaryKey("pk_sec_group_controls", x => x.id);
                    table.ForeignKey(
                        name: "fk_sec_group_controls_sec_control_lists_control_code",
                        column: x => x.control_code,
                        principalTable: "sec_control_lists",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_sec_group_controls_sec_groups_group_code",
                        column: x => x.group_code,
                        principalTable: "sec_groups",
                        principalColumn: "id");
                },
                comment: "SEC_GROUPS_CONTROLS table for linking groups and controls");

            migrationBuilder.CreateIndex(
                name: "ix_sec_control_lists_page_id",
                table: "sec_control_lists",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_control_code_group_code",
                table: "sec_group_controls",
                columns: new[] { "control_code", "group_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_controls_group_code",
                table: "sec_group_controls",
                column: "group_code");

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_employees_group_code_emp_code",
                table: "sec_group_employees",
                columns: new[] { "group_code", "emp_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_jobs_group_code_job_code",
                table: "sec_group_jobs",
                columns: new[] { "group_code", "job_code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_pages_group_code_page_id",
                table: "sec_group_pages",
                columns: new[] { "group_code", "page_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_sec_group_pages_page_id",
                table: "sec_group_pages",
                column: "page_id");

            migrationBuilder.CreateIndex(
                name: "ix_sec_groups_module_no",
                table: "sec_groups",
                column: "module_no");

            migrationBuilder.CreateIndex(
                name: "ix_sec_services_module_no",
                table: "sec_services",
                column: "module_no");

            migrationBuilder.CreateIndex(
                name: "ix_users_user_name",
                table: "users",
                column: "user_name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "base_structures");

            migrationBuilder.DropTable(
                name: "sec_group_controls");

            migrationBuilder.DropTable(
                name: "sec_group_employees");

            migrationBuilder.DropTable(
                name: "sec_group_jobs");

            migrationBuilder.DropTable(
                name: "sec_group_pages");

            migrationBuilder.DropTable(
                name: "sec_services");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "sec_control_lists");

            migrationBuilder.DropTable(
                name: "sec_groups");

            migrationBuilder.DropTable(
                name: "sec_pages");

            migrationBuilder.DropTable(
                name: "sec_modules");
        }
    }
}
