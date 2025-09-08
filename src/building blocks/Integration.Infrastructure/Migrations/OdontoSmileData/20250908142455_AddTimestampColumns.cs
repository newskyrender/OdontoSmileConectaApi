using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations.OdontoSmileData
{
    public partial class AddTimestampColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Adicionar colunas created_at e updated_at nas tabelas relacionais que já existem
            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "profissional_especialidades",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP()");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "profissional_especialidades",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "profissional_equipamentos",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP()");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "profissional_equipamentos",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "profissional_facilidades",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP()");

            migrationBuilder.AddColumn<DateTime>(
                name: "updated_at",
                table: "profissional_facilidades",
                type: "DATETIME",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP() ON UPDATE CURRENT_TIMESTAMP()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remover colunas created_at e updated_at das tabelas relacionais
            migrationBuilder.DropColumn(
                name: "created_at",
                table: "profissional_especialidades");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "profissional_especialidades");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "profissional_equipamentos");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "profissional_equipamentos");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "profissional_facilidades");

            migrationBuilder.DropColumn(
                name: "updated_at",
                table: "profissional_facilidades");
        }
    }
}
