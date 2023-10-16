using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoTuongBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOppnentAndRemoveRoomusers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomUser");

            migrationBuilder.DropColumn(
                name: "CountUser",
                table: "Rooms");

            migrationBuilder.AddColumn<Guid>(
                name: "OpponentUserId",
                table: "Rooms",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_OpponentUserId",
                table: "Rooms",
                column: "OpponentUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_OpponentUserId",
                table: "Rooms",
                column: "OpponentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_OpponentUserId",
                table: "Rooms");

            migrationBuilder.DropIndex(
                name: "IX_Rooms_OpponentUserId",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "OpponentUserId",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "CountUser",
                table: "Rooms",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateTable(
                name: "RoomUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsPlayer = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomUser", x => new { x.UserId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_RoomUser_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomUser_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomUser_RoomId",
                table: "RoomUser",
                column: "RoomId");
        }
    }
}
