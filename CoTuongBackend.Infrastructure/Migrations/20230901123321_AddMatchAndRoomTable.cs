using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoTuongBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddMatchAndRoomTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "AspNetUsers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Room",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CountUser = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    Password = table.Column<string>(type: "text", nullable: true),
                    HostUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Room", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Room_AspNetUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Match",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    HostUserId = table.Column<Guid>(type: "uuid", nullable: false),
                    RoomId = table.Column<Guid>(type: "uuid", nullable: false),
                    OpponentUserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    MatchDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Match", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Match_AspNetUsers_HostUserId",
                        column: x => x.HostUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_AspNetUsers_OpponentUserId",
                        column: x => x.OpponentUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Match_Room_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Room",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_RoomId",
                table: "AspNetUsers",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_HostUserId",
                table: "Match",
                column: "HostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_OpponentUserId",
                table: "Match",
                column: "OpponentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Match_RoomId",
                table: "Match",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Room_HostUserId",
                table: "Room",
                column: "HostUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Room_RoomId",
                table: "AspNetUsers",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Room_RoomId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Match");

            migrationBuilder.DropTable(
                name: "Room");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_RoomId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "AspNetUsers");
        }
    }
}
