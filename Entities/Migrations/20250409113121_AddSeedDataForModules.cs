using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Entities.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedDataForModules : Migration
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

            migrationBuilder.InsertData(
                table: "sec_modules",
                columns: new[] { "id", "color", "delete_date", "delete_user_code", "icon", "insert_date", "insert_user_code", "is_deleted", "is_taken", "last_update", "module_name", "update_user_code" },
                values: new object[,]
                {
                    { 40, null, null, null, null, null, null, false, true, null, "التأمين والسرية", null },
                    { 41, null, null, null, null, null, null, false, true, null, "الموارد البشرية", null },
                    { 43, null, null, null, null, null, null, false, true, null, "التكويد", null },
                    { 44, null, null, null, null, null, null, false, true, null, "الأرشيف", null },
                    { 45, null, null, null, null, null, null, false, true, null, "المخازن", null },
                    { 46, null, null, null, null, null, null, false, true, null, "خدمة العملاء", null },
                    { 47, null, null, null, null, null, null, false, true, null, "المالى", null },
                    { 48, null, null, null, null, null, null, false, true, null, "التدريب", null },
                    { 49, null, null, null, null, null, null, false, true, null, "العقود والمشتروات", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 40);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 41);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 43);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 44);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 45);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 46);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 47);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 48);

            migrationBuilder.DeleteData(
                table: "sec_modules",
                keyColumn: "id",
                keyValue: 49);

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
