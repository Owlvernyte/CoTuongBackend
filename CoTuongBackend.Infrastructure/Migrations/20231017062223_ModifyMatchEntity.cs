using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoTuongBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifyMatchEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_HostUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_OpponentUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Rooms_RoomId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_HostUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_OpponentUserId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_RoomId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "HostUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "OpponentUserId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "Matches");

            migrationBuilder.CreateTable(
                name: "UserMatch",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    MatchId = table.Column<Guid>(type: "uuid", nullable: false),
                    Result = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMatch", x => new { x.UserId, x.MatchId });
                    table.ForeignKey(
                        name: "FK_UserMatch_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserMatch_Matches_MatchId",
                        column: x => x.MatchId,
                        principalTable: "Matches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserMatch_MatchId",
                table: "UserMatch",
                column: "MatchId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserMatch");

            migrationBuilder.AddColumn<Guid>(
                name: "HostUserId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "OpponentUserId",
                table: "Matches",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "Matches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Matches_HostUserId",
                table: "Matches",
                column: "HostUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_OpponentUserId",
                table: "Matches",
                column: "OpponentUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Matches_RoomId",
                table: "Matches",
                column: "RoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_HostUserId",
                table: "Matches",
                column: "HostUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_AspNetUsers_OpponentUserId",
                table: "Matches",
                column: "OpponentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Rooms_RoomId",
                table: "Matches",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
