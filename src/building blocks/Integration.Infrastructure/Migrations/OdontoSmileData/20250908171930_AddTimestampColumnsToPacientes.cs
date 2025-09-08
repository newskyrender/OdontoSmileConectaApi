using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations.OdontoSmileData
{
    public partial class AddTimestampColumnsToPacientes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "id",
                keyValue: new Guid("0112ecdf-73f4-466f-a30b-cfa946330125"));

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "id", "ativo", "created_at", "email", "nome", "senha_hash", "updated_at" },
                values: new object[] { new Guid("c5fb5724-88d5-4860-819a-1901fff148e1"), true, new DateTime(2025, 9, 8, 17, 19, 29, 740, DateTimeKind.Utc).AddTicks(814), "admin@odontosmileddigital.com", "Administrador Sistema", "$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi", new DateTime(2025, 9, 8, 17, 19, 29, 740, DateTimeKind.Utc).AddTicks(815) });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "usuarios",
                keyColumn: "id",
                keyValue: new Guid("c5fb5724-88d5-4860-819a-1901fff148e1"));

            migrationBuilder.InsertData(
                table: "usuarios",
                columns: new[] { "id", "ativo", "created_at", "email", "nome", "senha_hash", "updated_at" },
                values: new object[] { new Guid("0112ecdf-73f4-466f-a30b-cfa946330125"), true, new DateTime(2025, 9, 8, 14, 24, 55, 729, DateTimeKind.Utc).AddTicks(3912), "admin@odontosmileddigital.com", "Administrador Sistema", "$2y$10$92IXUNpkjO0rOQ5byMi.Ye4oKoEa3Ro9llC/.og/at2.uheWG/igi", new DateTime(2025, 9, 8, 14, 24, 55, 729, DateTimeKind.Utc).AddTicks(3913) });
        }
    }
}
