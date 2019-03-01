using Microsoft.EntityFrameworkCore.Migrations;

namespace TTMMC.Migrations
{
    public partial class _23 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Layouts_LayoutsActRecords_LayoutSetRecordId",
                table: "Layouts");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutsActRecords_Layouts_LayoutId",
                table: "LayoutsActRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutsActRecordsFields_LayoutsActRecords_LayoutRecordId",
                table: "LayoutsActRecordsFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutsActRecordsFields",
                table: "LayoutsActRecordsFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutsActRecords",
                table: "LayoutsActRecords");

            migrationBuilder.RenameTable(
                name: "LayoutsActRecordsFields",
                newName: "LayoutsRecordsFields");

            migrationBuilder.RenameTable(
                name: "LayoutsActRecords",
                newName: "LayoutsRecords");

            migrationBuilder.RenameIndex(
                name: "IX_LayoutsActRecordsFields_LayoutRecordId",
                table: "LayoutsRecordsFields",
                newName: "IX_LayoutsRecordsFields_LayoutRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_LayoutsActRecords_LayoutId",
                table: "LayoutsRecords",
                newName: "IX_LayoutsRecords_LayoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "LayoutsRecordsFields",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "LayoutsRecordsFields",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutsRecordsFields",
                table: "LayoutsRecordsFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutsRecords",
                table: "LayoutsRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Layouts_LayoutsRecords_LayoutSetRecordId",
                table: "Layouts",
                column: "LayoutSetRecordId",
                principalTable: "LayoutsRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutsRecords_Layouts_LayoutId",
                table: "LayoutsRecords",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutsRecordsFields_LayoutsRecords_LayoutRecordId",
                table: "LayoutsRecordsFields",
                column: "LayoutRecordId",
                principalTable: "LayoutsRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Layouts_LayoutsRecords_LayoutSetRecordId",
                table: "Layouts");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutsRecords_Layouts_LayoutId",
                table: "LayoutsRecords");

            migrationBuilder.DropForeignKey(
                name: "FK_LayoutsRecordsFields_LayoutsRecords_LayoutRecordId",
                table: "LayoutsRecordsFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutsRecordsFields",
                table: "LayoutsRecordsFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LayoutsRecords",
                table: "LayoutsRecords");

            migrationBuilder.RenameTable(
                name: "LayoutsRecordsFields",
                newName: "LayoutsActRecordsFields");

            migrationBuilder.RenameTable(
                name: "LayoutsRecords",
                newName: "LayoutsActRecords");

            migrationBuilder.RenameIndex(
                name: "IX_LayoutsRecordsFields_LayoutRecordId",
                table: "LayoutsActRecordsFields",
                newName: "IX_LayoutsActRecordsFields_LayoutRecordId");

            migrationBuilder.RenameIndex(
                name: "IX_LayoutsRecords_LayoutId",
                table: "LayoutsActRecords",
                newName: "IX_LayoutsActRecords_LayoutId");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                table: "LayoutsActRecordsFields",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 255,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Key",
                table: "LayoutsActRecordsFields",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutsActRecordsFields",
                table: "LayoutsActRecordsFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LayoutsActRecords",
                table: "LayoutsActRecords",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Layouts_LayoutsActRecords_LayoutSetRecordId",
                table: "Layouts",
                column: "LayoutSetRecordId",
                principalTable: "LayoutsActRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutsActRecords_Layouts_LayoutId",
                table: "LayoutsActRecords",
                column: "LayoutId",
                principalTable: "Layouts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_LayoutsActRecordsFields_LayoutsActRecords_LayoutRecordId",
                table: "LayoutsActRecordsFields",
                column: "LayoutRecordId",
                principalTable: "LayoutsActRecords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
