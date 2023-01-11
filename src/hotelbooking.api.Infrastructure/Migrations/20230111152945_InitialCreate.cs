using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace hotelbooking.api.Infrastructure.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditLogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TableName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OldValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewValues = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AffectedColumns = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PrimaryKey = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MstFacility",
                columns: table => new
                {
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstFacility", x => x.FacilityId);
                });

            migrationBuilder.CreateTable(
                name: "MstRoom",
                columns: table => new
                {
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Pax = table.Column<int>(type: "int", maxLength: 2, nullable: false),
                    PricePerNight = table.Column<int>(type: "int", maxLength: 8, nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstRoom", x => x.RoomId);
                });

            migrationBuilder.CreateTable(
                name: "MstUser",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    HashedPassword = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MstUser", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "TrxRoomFacility",
                columns: table => new
                {
                    RoomFacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrxRoomFacility", x => x.RoomFacilityId);
                    table.ForeignKey(
                        name: "FK_TrxRoomFacility_MstFacility_FacilityId",
                        column: x => x.FacilityId,
                        principalTable: "MstFacility",
                        principalColumn: "FacilityId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrxRoomFacility_MstRoom_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MstRoom",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TrxBooking",
                columns: table => new
                {
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CheckInDate = table.Column<DateTime>(type: "date", nullable: false),
                    CheckOutDate = table.Column<DateTime>(type: "date", nullable: false),
                    CreatedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModifiedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeletedDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TrxBooking", x => x.BookingId);
                    table.ForeignKey(
                        name: "FK_TrxBooking_MstRoom_RoomId",
                        column: x => x.RoomId,
                        principalTable: "MstRoom",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TrxBooking_MstUser_UserId",
                        column: x => x.UserId,
                        principalTable: "MstUser",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MstFacility_CreatedDt",
                table: "MstFacility",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_MstFacility_LastModifiedDt",
                table: "MstFacility",
                column: "LastModifiedDt");

            migrationBuilder.CreateIndex(
                name: "IX_MstRoom_CreatedDt",
                table: "MstRoom",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_MstRoom_LastModifiedDt",
                table: "MstRoom",
                column: "LastModifiedDt");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_CreatedDt",
                table: "MstUser",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_MstUser_LastModifiedDt",
                table: "MstUser",
                column: "LastModifiedDt");

            migrationBuilder.CreateIndex(
                name: "IX_TrxBooking_CreatedDt",
                table: "TrxBooking",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_TrxBooking_LastModifiedDt",
                table: "TrxBooking",
                column: "LastModifiedDt");

            migrationBuilder.CreateIndex(
                name: "IX_TrxBooking_RoomId",
                table: "TrxBooking",
                column: "RoomId");

            migrationBuilder.CreateIndex(
                name: "IX_TrxBooking_UserId",
                table: "TrxBooking",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_TrxRoomFacility_CreatedDt",
                table: "TrxRoomFacility",
                column: "CreatedDt");

            migrationBuilder.CreateIndex(
                name: "IX_TrxRoomFacility_FacilityId",
                table: "TrxRoomFacility",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_TrxRoomFacility_LastModifiedDt",
                table: "TrxRoomFacility",
                column: "LastModifiedDt");

            migrationBuilder.CreateIndex(
                name: "IX_TrxRoomFacility_RoomId",
                table: "TrxRoomFacility",
                column: "RoomId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditLogs");

            migrationBuilder.DropTable(
                name: "TrxBooking");

            migrationBuilder.DropTable(
                name: "TrxRoomFacility");

            migrationBuilder.DropTable(
                name: "MstUser");

            migrationBuilder.DropTable(
                name: "MstFacility");

            migrationBuilder.DropTable(
                name: "MstRoom");
        }
    }
}
