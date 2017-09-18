using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Fabric.Realtime.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HL7Message",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ExternalPatientID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    InternalPatientID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MessageControlID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    MessageDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    MessageHash = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MessageType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProcessingID = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Protocol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ProtocolVersion = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RawMessage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReceivingApplication = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ReceivingFacility = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SendingApplication = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SendingFacility = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    TransmissionReceiptTime = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    XmlMessage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HL7Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subscription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Created = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    LastModified = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    MessageFormat = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    RoutingKey = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ForwardingHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageId = table.Column<long>(type: "bigint", nullable: false),
                    Sent = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ForwardingHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ForwardingHistory_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubscriptionMessageType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    MessageType = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    SubscriptionId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscriptionMessageType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscriptionMessageType_Subscription_SubscriptionId",
                        column: x => x.SubscriptionId,
                        principalTable: "Subscription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ForwardingHistory_SubscriptionId",
                table: "ForwardingHistory",
                column: "SubscriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionMessageType_SubscriptionId",
                table: "SubscriptionMessageType",
                column: "SubscriptionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ForwardingHistory");

            migrationBuilder.DropTable(
                name: "HL7Message");

            migrationBuilder.DropTable(
                name: "SubscriptionMessageType");

            migrationBuilder.DropTable(
                name: "Subscription");
        }
    }
}
