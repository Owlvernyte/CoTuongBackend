using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoTuongBackend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddRoomAndMatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Room_RoomId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_AspNetUsers_HostUserId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_AspNetUsers_OpponentUserId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Match_Room_RoomId",
                table: "Match");

            migrationBuilder.DropForeignKey(
                name: "FK_Room_AspNetUsers_HostUserId",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Room",
                table: "Room");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Match",
                table: "Match");

            migrationBuilder.RenameTable(
                name: "Room",
                newName: "Rooms");

            migrationBuilder.RenameTable(
                name: "Match",
                newName: "Matches");

            migrationBuilder.RenameIndex(
                name: "IX_Room_HostUserId",
                table: "Rooms",
                newName: "IX_Rooms_HostUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_RoomId",
                table: "Matches",
                newName: "IX_Matches_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_OpponentUserId",
                table: "Matches",
                newName: "IX_Matches_OpponentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Match_HostUserId",
                table: "Matches",
                newName: "IX_Matches_HostUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Matches",
                table: "Matches",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomId",
                table: "AspNetUsers",
                column: "RoomId",
                principalTable: "Rooms",
                principalColumn: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rooms_AspNetUsers_HostUserId",
                table: "Rooms",
                column: "HostUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Rooms_RoomId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_HostUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_AspNetUsers_OpponentUserId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Rooms_RoomId",
                table: "Matches");

            migrationBuilder.DropForeignKey(
                name: "FK_Rooms_AspNetUsers_HostUserId",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rooms",
                table: "Rooms");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Matches",
                table: "Matches");

            migrationBuilder.RenameTable(
                name: "Rooms",
                newName: "Room");

            migrationBuilder.RenameTable(
                name: "Matches",
                newName: "Match");

            migrationBuilder.RenameIndex(
                name: "IX_Rooms_HostUserId",
                table: "Room",
                newName: "IX_Room_HostUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_RoomId",
                table: "Match",
                newName: "IX_Match_RoomId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_OpponentUserId",
                table: "Match",
                newName: "IX_Match_OpponentUserId");

            migrationBuilder.RenameIndex(
                name: "IX_Matches_HostUserId",
                table: "Match",
                newName: "IX_Match_HostUserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Room",
                table: "Room",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Match",
                table: "Match",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Room_RoomId",
                table: "AspNetUsers",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Match_AspNetUsers_HostUserId",
                table: "Match",
                column: "HostUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_AspNetUsers_OpponentUserId",
                table: "Match",
                column: "OpponentUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Match_Room_RoomId",
                table: "Match",
                column: "RoomId",
                principalTable: "Room",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Room_AspNetUsers_HostUserId",
                table: "Room",
                column: "HostUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
