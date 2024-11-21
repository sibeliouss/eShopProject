using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class edited_configuration_pd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UpdateAt",
                table: "ProductDiscounts",
                newName: "Update Date");

            migrationBuilder.RenameColumn(
                name: "CreateAt",
                table: "ProductDiscounts",
                newName: "Create Date");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Update Date",
                table: "ProductDiscounts",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Create Date",
                table: "ProductDiscounts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Update Date",
                table: "ProductDiscounts",
                newName: "UpdateAt");

            migrationBuilder.RenameColumn(
                name: "Create Date",
                table: "ProductDiscounts",
                newName: "CreateAt");

            migrationBuilder.AlterColumn<DateTime>(
                name: "UpdateAt",
                table: "ProductDiscounts",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true,
                oldDefaultValueSql: "GETDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateAt",
                table: "ProductDiscounts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
