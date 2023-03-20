using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Worktime.Core.Migrations
{
    /// <inheritdoc />
    public partial class Update_20_03_2023 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Tasks_WTTaskId",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_WTUserId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "WTUserId",
                table: "Tasks",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "WTTaskId",
                table: "Lines",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Tasks_WTTaskId",
                table: "Lines",
                column: "WTTaskId",
                principalTable: "Tasks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_WTUserId",
                table: "Tasks",
                column: "WTUserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Lines_Tasks_WTTaskId",
                table: "Lines");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_WTUserId",
                table: "Tasks");

            migrationBuilder.AlterColumn<Guid>(
                name: "WTUserId",
                table: "Tasks",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<int>(
                name: "WTTaskId",
                table: "Lines",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_Lines_Tasks_WTTaskId",
                table: "Lines",
                column: "WTTaskId",
                principalTable: "Tasks",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_WTUserId",
                table: "Tasks",
                column: "WTUserId",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
