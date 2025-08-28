using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Integration.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CompleteSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "banks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bank_number = table.Column<int>(type: "int", nullable: false),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_banks", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "companies",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    company_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    trading_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cnpj = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_companies", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "documentos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    entidade_tipo = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    entidade_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    tipo_documento = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_original = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_arquivo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    caminho_arquivo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tamanho_bytes = table.Column<int>(type: "int", nullable: false),
                    tipo_mime = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "longtext", nullable: false, defaultValue: "Pendente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_documentos", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Fake",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Nome = table.Column<string>(type: "VARCHAR(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "VARCHAR(1000)", maxLength: 1000, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fake", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "pacientes",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    nome_completo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_nascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    estado_civil = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sexo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    profissao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    celular_principal = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone_fixo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contato_emergencia = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cep = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    endereco_completo = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bairro = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidade = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complemento = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero_cooperado = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    situacao_cooperativa = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    renda_mensal = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    limite_disponivel = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    limite_total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    limite_utilizado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, defaultValue: 0m),
                    status = table.Column<string>(type: "longtext", nullable: false, defaultValue: "Ativo")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pacientes", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profissionais",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    nome_completo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_nascimento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    sexo = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email_profissional = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    celular = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone_adicional = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cro = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_formatura = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    universidade_formacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tempo_experiencia = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    outras_especialidades = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_consultorio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cnpj = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone_consultorio = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cep_consultorio = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    endereco_consultorio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bairro_consultorio = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cidade_consultorio = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estado_consultorio = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    complemento_consultorio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero_consultorio = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    numero_cadeiras = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    outros_equipamentos = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observacoes_consultorio = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    segunda_sexta_inicio = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    segunda_sexta_fim = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    sabado_inicio = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    sabado_fim = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    domingo_inicio = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    domingo_fim = table.Column<TimeSpan>(type: "time(6)", nullable: true),
                    tempo_medio_consulta = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    banco = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_conta = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    agencia = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    conta = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome_titular = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf_titular = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pergunta_seguranca = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    resposta_seguranca_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    termos_uso = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    codigo_etica = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    responsabilidade = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    dados_pessoais = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    marketing = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    status_aprovacao = table.Column<string>(type: "varchar(255)", nullable: false, defaultValue: "Pendente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profissionais", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    senha_hash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bank_accounts",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    account_number = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_digit = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    agency = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    account_type = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    bank_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    BankId1 = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_bank_accounts", x => x.id);
                    table.ForeignKey(
                        name: "FK_bank_accounts_banks_BankId1",
                        column: x => x.BankId1,
                        principalTable: "banks",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_bank_accounts_banks_bank_id",
                        column: x => x.bank_id,
                        principalTable: "banks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agendamentos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    paciente_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    paciente_nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    data_agendamento = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    horario_inicio = table.Column<TimeSpan>(type: "time(6)", nullable: false),
                    duracao_minutos = table.Column<int>(type: "int", nullable: false, defaultValue: 60),
                    servico = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(255)", nullable: false, defaultValue: "Agendado")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observacoes = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observacoes_cancelamento = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agendamentos", x => x.id);
                    table.ForeignKey(
                        name: "FK_agendamentos_pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_agendamentos_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profissional_equipamentos",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    equipamento = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profissional_equipamentos", x => x.id);
                    table.ForeignKey(
                        name: "FK_profissional_equipamentos_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profissional_especialidades",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    especialidade = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profissional_especialidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_profissional_especialidades_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profissional_facilidades",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    facilidade = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_profissional_facilidades", x => x.id);
                    table.ForeignKey(
                        name: "FK_profissional_facilidades_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "solicitacoes_orcamento",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    numero_pedido = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    paciente_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    nome_completo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cpf = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tipo_tratamento = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    observacoes = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(255)", nullable: false, defaultValue: "Pendente")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    valor_aprovado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    numero_parcelas = table.Column<int>(type: "int", nullable: true),
                    valor_parcela = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_solicitacoes_orcamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_solicitacoes_orcamento_pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_solicitacoes_orcamento_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "planejamentos_digitais",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    solicitacao_orcamento_id = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    paciente_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    profissional_id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    numero_alinhadores = table.Column<int>(type: "int", nullable: false),
                    duracao_tratamento_meses = table.Column<int>(type: "int", nullable: false),
                    observacoes = table.Column<string>(type: "TEXT", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    orcamento_estimado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    tipo_aparelho = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    prioridade_caso = table.Column<string>(type: "longtext", nullable: false, defaultValue: "Normal")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(255)", nullable: false, defaultValue: "Rascunho")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dias_entre_trocas = table.Column<int>(type: "int", nullable: true),
                    consultas_acompanhamento = table.Column<int>(type: "int", nullable: true),
                    created_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)"),
                    updated_at = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_planejamentos_digitais", x => x.id);
                    table.ForeignKey(
                        name: "FK_planejamentos_digitais_pacientes_paciente_id",
                        column: x => x.paciente_id,
                        principalTable: "pacientes",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_planejamentos_digitais_profissionais_profissional_id",
                        column: x => x.profissional_id,
                        principalTable: "profissionais",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_planejamentos_digitais_solicitacoes_orcamento_solicitacao_or~",
                        column: x => x.solicitacao_orcamento_id,
                        principalTable: "solicitacoes_orcamento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_agendamentos_data_agendamento",
                table: "agendamentos",
                column: "data_agendamento");

            migrationBuilder.CreateIndex(
                name: "IX_agendamentos_paciente_id",
                table: "agendamentos",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_agendamentos_profissional_id_data_agendamento",
                table: "agendamentos",
                columns: new[] { "profissional_id", "data_agendamento" });

            migrationBuilder.CreateIndex(
                name: "IX_agendamentos_status",
                table: "agendamentos",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_bank_id_account_number_account_digit",
                table: "bank_accounts",
                columns: new[] { "bank_id", "account_number", "account_digit" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_bank_accounts_BankId1",
                table: "bank_accounts",
                column: "BankId1");

            migrationBuilder.CreateIndex(
                name: "IX_banks_bank_number",
                table: "banks",
                column: "bank_number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_cnpj",
                table: "companies",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companies_email",
                table: "companies",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_documentos_entidade_tipo_entidade_id",
                table: "documentos",
                columns: new[] { "entidade_tipo", "entidade_id" });

            migrationBuilder.CreateIndex(
                name: "IX_documentos_tipo_documento",
                table: "documentos",
                column: "tipo_documento");

            migrationBuilder.CreateIndex(
                name: "IX_pacientes_cpf",
                table: "pacientes",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_pacientes_email",
                table: "pacientes",
                column: "email");

            migrationBuilder.CreateIndex(
                name: "IX_pacientes_numero_cooperado",
                table: "pacientes",
                column: "numero_cooperado");

            migrationBuilder.CreateIndex(
                name: "IX_planejamentos_digitais_paciente_id",
                table: "planejamentos_digitais",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_planejamentos_digitais_profissional_id",
                table: "planejamentos_digitais",
                column: "profissional_id");

            migrationBuilder.CreateIndex(
                name: "IX_planejamentos_digitais_solicitacao_orcamento_id",
                table: "planejamentos_digitais",
                column: "solicitacao_orcamento_id");

            migrationBuilder.CreateIndex(
                name: "IX_planejamentos_digitais_status",
                table: "planejamentos_digitais",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_planejamentos_digitais_tipo_aparelho",
                table: "planejamentos_digitais",
                column: "tipo_aparelho");

            migrationBuilder.CreateIndex(
                name: "IX_profissionais_cpf",
                table: "profissionais",
                column: "cpf",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profissionais_cro",
                table: "profissionais",
                column: "cro",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profissionais_email_profissional",
                table: "profissionais",
                column: "email_profissional",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profissionais_status_aprovacao",
                table: "profissionais",
                column: "status_aprovacao");

            migrationBuilder.CreateIndex(
                name: "IX_profissional_equipamentos_profissional_id_equipamento",
                table: "profissional_equipamentos",
                columns: new[] { "profissional_id", "equipamento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profissional_especialidades_profissional_id_especialidade",
                table: "profissional_especialidades",
                columns: new[] { "profissional_id", "especialidade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_profissional_facilidades_profissional_id_facilidade",
                table: "profissional_facilidades",
                columns: new[] { "profissional_id", "facilidade" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_solicitacoes_orcamento_cpf",
                table: "solicitacoes_orcamento",
                column: "cpf");

            migrationBuilder.CreateIndex(
                name: "IX_solicitacoes_orcamento_numero_pedido",
                table: "solicitacoes_orcamento",
                column: "numero_pedido",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_solicitacoes_orcamento_paciente_id",
                table: "solicitacoes_orcamento",
                column: "paciente_id");

            migrationBuilder.CreateIndex(
                name: "IX_solicitacoes_orcamento_profissional_id",
                table: "solicitacoes_orcamento",
                column: "profissional_id");

            migrationBuilder.CreateIndex(
                name: "IX_solicitacoes_orcamento_status",
                table: "solicitacoes_orcamento",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_users_email",
                table: "users",
                column: "email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_email",
                table: "usuarios",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "agendamentos");

            migrationBuilder.DropTable(
                name: "bank_accounts");

            migrationBuilder.DropTable(
                name: "companies");

            migrationBuilder.DropTable(
                name: "documentos");

            migrationBuilder.DropTable(
                name: "Fake");

            migrationBuilder.DropTable(
                name: "planejamentos_digitais");

            migrationBuilder.DropTable(
                name: "profissional_equipamentos");

            migrationBuilder.DropTable(
                name: "profissional_especialidades");

            migrationBuilder.DropTable(
                name: "profissional_facilidades");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "banks");

            migrationBuilder.DropTable(
                name: "solicitacoes_orcamento");

            migrationBuilder.DropTable(
                name: "pacientes");

            migrationBuilder.DropTable(
                name: "profissionais");
        }
    }
}
