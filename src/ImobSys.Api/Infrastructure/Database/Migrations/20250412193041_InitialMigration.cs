using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ImobSys.Api.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "admin");

            migrationBuilder.CreateTable(
                name: "propertiesTypes",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_properties_types", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "properties",
                schema: "admin",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    type_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_properties", x => x.id);
                    table.ForeignKey(
                        name: "fk_properties_properties_types_type_id",
                        column: x => x.type_id,
                        principalSchema: "admin",
                        principalTable: "propertiesTypes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                schema: "admin",
                table: "propertiesTypes",
                columns: new[] { "id", "description", "name" },
                values: new object[,]
                {
                    { new Guid("0366f790-41f6-476a-9780-e648f58b3eaf"), "Casa", "Casa" },
                    { new Guid("3cf4eb65-8052-48f9-9c49-2e09b83f053d"), "Apartamento duplex", "Apartamento duplex" },
                    { new Guid("4d84993a-4139-4265-a934-8952c20bbf56"), "Apartamento padrão", "Apartamento padrão" },
                    { new Guid("5c39a928-2f9c-4d41-95f8-8ef46e9dc15c"), "Casa em condomínio fechado", "Casa em condomínio" },
                    { new Guid("a10f2f8c-3bce-4e8f-b8bb-69ffb62a2b41"), "Casa duplex", "Casa duplex" },
                    { new Guid("df1b2717-84cd-496a-ae6f-5eaa601cc879"), "Cobertura", "Cobertura" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_properties_name_description",
                schema: "admin",
                table: "properties",
                columns: new[] { "name", "description" });

            migrationBuilder.CreateIndex(
                name: "ix_properties_type_id",
                schema: "admin",
                table: "properties",
                column: "type_id");

            migrationBuilder.CreateIndex(
                name: "ix_properties_types_name",
                schema: "admin",
                table: "propertiesTypes",
                column: "name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "properties",
                schema: "admin");

            migrationBuilder.DropTable(
                name: "propertiesTypes",
                schema: "admin");
        }
    }
}
