using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations.Oracle
{
    /// <inheritdoc />
    public partial class initialOracle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AUDIT_LOGS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    TABLE_ID = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false),
                    TIME_STAMP = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    EMP_SERIAL = table.Column<string>(type: "NVARCHAR2(120)", maxLength: 120, nullable: true),
                    TABLE_NAME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true),
                    CHANGES = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: true),
                    MODULE_NO = table.Column<decimal>(type: "DECIMAL(5)", precision: 5, nullable: true),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AUDIT_LOGS", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "SEC_GROUPS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    GROUP_NAME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Name of the group"),
                    ARCHIVE_ROLE = table.Column<int>(type: "NUMBER(10)", nullable: true, comment: "Role for archive permissions"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_GROUPS", x => x.ID);
                },
                comment: "SEC_GROUPS table for storing group information");

            migrationBuilder.CreateTable(
                name: "SEC_MODULES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    MODULE_NAME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Name of the module"),
                    ICON = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true, comment: "Icon of the module"),
                    COLOR = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true, comment: "Color of the module"),
                    IS_TAKEN = table.Column<int>(type: "NUMBER(1,0)", nullable: true, defaultValue: 0),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_MODULES", x => x.ID);
                },
                comment: "SEC_MODULES table for storing module information");

            migrationBuilder.CreateTable(
                name: "SIGNATURES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    SIGNATURE_COLOR = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false),
                    EMPLOYEE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SIGNATURES", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "USERS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    USER_NAME = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: false, comment: "Name of the user"),
                    USER_PASSWORD = table.Column<string>(type: "NVARCHAR2(500)", maxLength: 500, nullable: false, comment: "Encrypted password for the user"),
                    USER_DESC = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true, comment: "Description of the user"),
                    PERSON_CODE = table.Column<string>(type: "NVARCHAR2(20)", maxLength: 20, nullable: true, comment: "Code associated with the person"),
                    MEMO = table.Column<string>(type: "NVARCHAR2(255)", maxLength: 255, nullable: true, comment: "Additional notes or memo for the user"),
                    MTG_PERSON_ROLE = table.Column<int>(type: "NUMBER(10)", nullable: true, comment: "Role of the person in meetings"),
                    MTG_REL_PRSN = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true, comment: "Related person information for meetings"),
                    USER_ACTIVATION = table.Column<int>(type: "NUMBER(10)", nullable: true, defaultValueSql: "1", comment: "Activation status of the user (default: 1)"),
                    VERSION_NO = table.Column<int>(type: "NUMBER(10)", nullable: true, comment: "Version number for the user"),
                    SIGN = table.Column<string>(type: "NVARCHAR2(50)", maxLength: 50, nullable: true, defaultValue: "empty", comment: "Digital signature of the user"),
                    SIGNOTECH = table.Column<int>(type: "NUMBER(10)", nullable: true, defaultValueSql: "0", comment: "Flag for Signotec (default: 0)"),
                    EMP_SERIAL = table.Column<int>(type: "NUMBER(10)", nullable: true, comment: "Employee serial number associated with the user"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USERS", x => x.ID);
                },
                comment: "USERS table for storing user information");

            migrationBuilder.CreateTable(
                name: "SEC_GROUP_EMPLOYEES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    GROUP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Group code referencing a group"),
                    EMP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Employee code referencing an employee"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_GROUP_EMPLOYEES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_EMPLOYEES_SEC_GROUPS_GROUP_CODE",
                        column: x => x.GROUP_CODE,
                        principalTable: "SEC_GROUPS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_GROUPS_EMPLOYEES table for linking groups and employees");

            migrationBuilder.CreateTable(
                name: "SEC_GROUP_JOBS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    GROUP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Group code referencing a group"),
                    JOB_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Job code referencing a job"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_GROUP_JOBS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_JOBS_SEC_GROUPS_GROUP_CODE",
                        column: x => x.GROUP_CODE,
                        principalTable: "SEC_GROUPS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_GROUPS_JOBS table for linking groups and jobs");

            migrationBuilder.CreateTable(
                name: "SEC_MODULE_GROUPS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    MODULE_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    GROUP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_MODULE_GROUPS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_MODULE_GROUPS_SEC_GROUPS_GROUP_CODE",
                        column: x => x.GROUP_CODE,
                        principalTable: "SEC_GROUPS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_MODULE_GROUPS_SEC_MODULES_MODULE_CODE",
                        column: x => x.MODULE_CODE,
                        principalTable: "SEC_MODULES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SEC_SERVICES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    MODULE_NO = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    SERVICE_NAME = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Name of the SERVICES "),
                    IS_TAKEN = table.Column<int>(type: "NUMBER(1,0)", nullable: true, defaultValue: 0),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_SERVICES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_SERVICES_SEC_MODULES_MODULE_NO",
                        column: x => x.MODULE_NO,
                        principalTable: "SEC_MODULES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_SERVICES table for storing SERVICES information");

            migrationBuilder.CreateTable(
                name: "SEC_PAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    PAGE_NAME = table.Column<string>(type: "NVARCHAR2(100)", maxLength: 100, nullable: false, comment: "Page name in English"),
                    PARENT_ID = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    PAGE_ORDER = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Order of the page"),
                    PAGE_URL = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Url associated with the page"),
                    ICON = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: true, comment: "Icon associated with the page"),
                    MODULE_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Module number associated with the page"),
                    SERVICE_CODE = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_PAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_PAGES_SEC_MODULES_MODULE_CODE",
                        column: x => x.MODULE_CODE,
                        principalTable: "SEC_MODULES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_PAGES_SEC_PAGES_PARENT_ID",
                        column: x => x.PARENT_ID,
                        principalTable: "SEC_PAGES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_PAGES_SEC_SERVICES_SERVICE_CODE",
                        column: x => x.SERVICE_CODE,
                        principalTable: "SEC_SERVICES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.SetNull);
                },
                comment: "SEC_PAGES table for storing page details");

            migrationBuilder.CreateTable(
                name: "SEC_CONTROL_LISTS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    CONTROL_CODE = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Name of the control"),
                    CONTROL_DESCRIPTION = table.Column<string>(type: "NVARCHAR2(200)", maxLength: 200, nullable: false, comment: "Description of the control"),
                    PAGE_ID = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Foreign key referencing SEC_PAGES table"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_CONTROL_LISTS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_CONTROL_LISTS_SEC_PAGES_PAGE_ID",
                        column: x => x.PAGE_ID,
                        principalTable: "SEC_PAGES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_CONTROLS_LIST table for storing control information");

            migrationBuilder.CreateTable(
                name: "SEC_GROUP_PAGES",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    GROUP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Group code referencing a group"),
                    PAGE_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Page ID referencing a page"),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_GROUP_PAGES", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_PAGES_SEC_GROUPS_GROUP_CODE",
                        column: x => x.GROUP_CODE,
                        principalTable: "SEC_GROUPS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_PAGES_SEC_PAGES_PAGE_CODE",
                        column: x => x.PAGE_CODE,
                        principalTable: "SEC_PAGES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_GROUPS_PAGES table for linking groups and pages");

            migrationBuilder.CreateTable(
                name: "SEC_GROUP_CONTROLS",
                columns: table => new
                {
                    ID = table.Column<int>(type: "NUMBER(10)", nullable: false)
                        ,
                    CONTROL_ID = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Control code, foreign key referencing SEC_CONTROLS_LIST"),
                    GROUP_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false, comment: "Group code, foreign key referencing SEC_GROUPS"),
                    PAGE_CODE = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    INSERT_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    INSERT_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    UPDATE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    LAST_UPDATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true),
                    IS_DELETED = table.Column<int>(type: "NUMBER(1,0)", nullable: false),
                    DELETE_USER_CODE = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    DELETE_DATE = table.Column<DateTime>(type: "TIMESTAMP(7)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SEC_GROUP_CONTROLS", x => x.ID);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_CONTROLS_SEC_CONTROL_LISTS_CONTROL_ID",
                        column: x => x.CONTROL_ID,
                        principalTable: "SEC_CONTROL_LISTS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_CONTROLS_SEC_GROUPS_GROUP_CODE",
                        column: x => x.GROUP_CODE,
                        principalTable: "SEC_GROUPS",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SEC_GROUP_CONTROLS_SEC_PAGES_PAGE_CODE",
                        column: x => x.PAGE_CODE,
                        principalTable: "SEC_PAGES",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "SEC_GROUPS_CONTROLS table for linking groups and controls");

            migrationBuilder.InsertData(
                table: "SEC_MODULES",
                columns: new[] { "ID", "COLOR", "DELETE_DATE", "DELETE_USER_CODE", "ICON", "INSERT_DATE", "INSERT_USER_CODE", "IS_DELETED", "IS_TAKEN", "LAST_UPDATE", "MODULE_NAME", "UPDATE_USER_CODE" },
                values: new object[,]
                {
                    { 40, null, null, null, null, null, null, 0, 1, null, "التأمين والسرية", null },
                    { 41, null, null, null, null, null, null, 0, 1, null, "الموارد البشرية", null },
                    { 43, null, null, null, null, null, null, 0, 1, null, "التكويد", null },
                    { 44, null, null, null, null, null, null, 0, 1, null, "الأرشيف", null },
                    { 45, null, null, null, null, null, null, 0, 1, null, "المخازن", null },
                    { 46, null, null, null, null, null, null, 0, 1, null, "خدمة العملاء", null },
                    { 47, null, null, null, null, null, null, 0, 1, null, "المالى", null },
                    { 48, null, null, null, null, null, null, 0, 1, null, "التدريب", null },
                    { 49, null, null, null, null, null, null, 0, 1, null, "العقود والمشتروات", null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_SEC_CONTROL_LISTS_CONTROL_CODE",
                table: "SEC_CONTROL_LISTS",
                column: "CONTROL_CODE",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_CONTROL_LISTS_PAGE_ID",
                table: "SEC_CONTROL_LISTS",
                column: "PAGE_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_CONTROLS_CONTROL_ID",
                table: "SEC_GROUP_CONTROLS",
                column: "CONTROL_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_CONTROLS_GROUP_CODE_CONTROL_ID_PAGE_CODE",
                table: "SEC_GROUP_CONTROLS",
                columns: new[] { "GROUP_CODE", "CONTROL_ID", "PAGE_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_CONTROLS_PAGE_CODE",
                table: "SEC_GROUP_CONTROLS",
                column: "PAGE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_EMPLOYEES_GROUP_CODE_EMP_CODE",
                table: "SEC_GROUP_EMPLOYEES",
                columns: new[] { "GROUP_CODE", "EMP_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_JOBS_GROUP_CODE_JOB_CODE",
                table: "SEC_GROUP_JOBS",
                columns: new[] { "GROUP_CODE", "JOB_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_PAGES_GROUP_CODE_PAGE_CODE",
                table: "SEC_GROUP_PAGES",
                columns: new[] { "GROUP_CODE", "PAGE_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_GROUP_PAGES_PAGE_CODE",
                table: "SEC_GROUP_PAGES",
                column: "PAGE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_MODULE_GROUPS_GROUP_CODE_MODULE_CODE",
                table: "SEC_MODULE_GROUPS",
                columns: new[] { "GROUP_CODE", "MODULE_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_MODULE_GROUPS_MODULE_CODE",
                table: "SEC_MODULE_GROUPS",
                column: "MODULE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_PAGES_MODULE_CODE",
                table: "SEC_PAGES",
                column: "MODULE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_PAGES_PAGE_ORDER_MODULE_CODE",
                table: "SEC_PAGES",
                columns: new[] { "PAGE_ORDER", "MODULE_CODE" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SEC_PAGES_PARENT_ID",
                table: "SEC_PAGES",
                column: "PARENT_ID");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_PAGES_SERVICE_CODE",
                table: "SEC_PAGES",
                column: "SERVICE_CODE");

            migrationBuilder.CreateIndex(
                name: "IX_SEC_SERVICES_MODULE_NO",
                table: "SEC_SERVICES",
                column: "MODULE_NO");

            migrationBuilder.CreateIndex(
                name: "IX_USERS_USER_NAME",
                table: "USERS",
                column: "USER_NAME",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AUDIT_LOGS");

            migrationBuilder.DropTable(
                name: "SEC_GROUP_CONTROLS");

            migrationBuilder.DropTable(
                name: "SEC_GROUP_EMPLOYEES");

            migrationBuilder.DropTable(
                name: "SEC_GROUP_JOBS");

            migrationBuilder.DropTable(
                name: "SEC_GROUP_PAGES");

            migrationBuilder.DropTable(
                name: "SEC_MODULE_GROUPS");

            migrationBuilder.DropTable(
                name: "SIGNATURES");

            migrationBuilder.DropTable(
                name: "USERS");

            migrationBuilder.DropTable(
                name: "SEC_CONTROL_LISTS");

            migrationBuilder.DropTable(
                name: "SEC_GROUPS");

            migrationBuilder.DropTable(
                name: "SEC_PAGES");

            migrationBuilder.DropTable(
                name: "SEC_SERVICES");

            migrationBuilder.DropTable(
                name: "SEC_MODULES");
        }
    }
}
