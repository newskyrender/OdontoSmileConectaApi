using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Integration.Domain.Entities;
using Integration.Domain.Enums;

namespace Integration.Infrastructure.Mappings
{
    public class UsuarioMap : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> entity)
        {
            entity.ToTable("usuarios");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(x => x.SenhaHash).HasColumnName("senha_hash").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Nome).HasColumnName("nome").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Ativo).HasColumnName("ativo").HasDefaultValue(true);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            entity.HasIndex(x => x.Email).IsUnique();
        }
    }

    public class PacienteMap : IEntityTypeConfiguration<Paciente>
    {
        public void Configure(EntityTypeBuilder<Paciente> entity)
        {
            entity.ToTable("pacientes");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            // Dados Pessoais
            entity.Property(x => x.NomeCompleto).HasColumnName("nome_completo").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Cpf).HasColumnName("cpf").IsRequired().HasMaxLength(14);
            entity.Property(x => x.DataNascimento).HasColumnName("data_nascimento").IsRequired();
            entity.Property(x => x.EstadoCivil).HasColumnName("estado_civil").HasConversion<string>().IsRequired();
            entity.Property(x => x.Sexo).HasColumnName("sexo").HasConversion<string>().IsRequired();
            entity.Property(x => x.Profissao).HasColumnName("profissao").HasMaxLength(100);

            // Dados de Contato
            entity.Property(x => x.CelularPrincipal).HasColumnName("celular_principal").IsRequired().HasMaxLength(20);
            entity.Property(x => x.TelefoneFixo).HasColumnName("telefone_fixo").HasMaxLength(20);
            entity.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(x => x.ContatoEmergencia).HasColumnName("contato_emergencia").HasMaxLength(20);

            // Endereço
            entity.Property(x => x.Cep).HasColumnName("cep").IsRequired().HasMaxLength(10);
            entity.Property(x => x.EnderecoCompleto).HasColumnName("endereco_completo").IsRequired().HasMaxLength(500);
            entity.Property(x => x.Bairro).HasColumnName("bairro").IsRequired().HasMaxLength(100);
            entity.Property(x => x.Cidade).HasColumnName("cidade").IsRequired().HasMaxLength(100);
            entity.Property(x => x.Estado).HasColumnName("estado").IsRequired().HasMaxLength(2);
            entity.Property(x => x.Complemento).HasColumnName("complemento").HasMaxLength(200);
            entity.Property(x => x.Numero).HasColumnName("numero").HasMaxLength(20);

            // Dados COOP1000
            entity.Property(x => x.NumeroCooperado).HasColumnName("numero_cooperado").HasMaxLength(50);
            entity.Property(x => x.SituacaoCooperativa).HasColumnName("situacao_cooperativa").HasMaxLength(50);
            entity.Property(x => x.RendaMensal).HasColumnName("renda_mensal").HasPrecision(10, 2);
            entity.Property(x => x.LimiteDisponivel).HasColumnName("limite_disponivel").HasPrecision(10, 2);
            entity.Property(x => x.LimiteTotal).HasColumnName("limite_total").HasPrecision(10, 2);
            entity.Property(x => x.LimiteUtilizado).HasColumnName("limite_utilizado").HasPrecision(10, 2).HasDefaultValue(0);

            // Controle
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(StatusPaciente.Ativo);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Índices
            entity.HasIndex(x => x.Cpf).IsUnique();
            entity.HasIndex(x => x.Email);
            entity.HasIndex(x => x.NumeroCooperado);
        }
    }

    public class ProfissionalMap : IEntityTypeConfiguration<Profissional>
    {
        public void Configure(EntityTypeBuilder<Profissional> entity)
        {
            entity.ToTable("profissionais");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();

            // Dados Pessoais
            entity.Property(x => x.NomeCompleto).HasColumnName("nome_completo").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Cpf).HasColumnName("cpf").IsRequired().HasMaxLength(14);
            entity.Property(x => x.DataNascimento).HasColumnName("data_nascimento").IsRequired();
            entity.Property(x => x.Sexo).HasColumnName("sexo").HasConversion<string>().IsRequired();
            entity.Property(x => x.EmailProfissional).HasColumnName("email_profissional").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Celular).HasColumnName("celular").IsRequired().HasMaxLength(20);
            entity.Property(x => x.TelefoneAdicional).HasColumnName("telefone_adicional").HasMaxLength(20);

            // Dados Profissionais
            entity.Property(x => x.Cro).HasColumnName("cro").IsRequired().HasMaxLength(50);
            entity.Property(x => x.DataFormatura).HasColumnName("data_formatura").IsRequired();
            entity.Property(x => x.UniversidadeFormacao).HasColumnName("universidade_formacao").IsRequired().HasMaxLength(255);
            entity.Property(x => x.TempoExperiencia).HasColumnName("tempo_experiencia").HasConversion<string>().IsRequired();
            entity.Property(x => x.OutrasEspecialidades).HasColumnName("outras_especialidades").HasColumnType("TEXT");

            // Dados do Consultório
            entity.Property(x => x.NomeConsultorio).HasColumnName("nome_consultorio").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Cnpj).HasColumnName("cnpj").HasMaxLength(18);
            entity.Property(x => x.TelefoneConsultorio).HasColumnName("telefone_consultorio").IsRequired().HasMaxLength(20);
            entity.Property(x => x.CepConsultorio).HasColumnName("cep_consultorio").IsRequired().HasMaxLength(10);
            entity.Property(x => x.EnderecoConsultorio).HasColumnName("endereco_consultorio").IsRequired().HasMaxLength(500);
            entity.Property(x => x.BairroConsultorio).HasColumnName("bairro_consultorio").IsRequired().HasMaxLength(100);
            entity.Property(x => x.CidadeConsultorio).HasColumnName("cidade_consultorio").IsRequired().HasMaxLength(100);
            entity.Property(x => x.EstadoConsultorio).HasColumnName("estado_consultorio").IsRequired().HasMaxLength(2);
            entity.Property(x => x.ComplementoConsultorio).HasColumnName("complemento_consultorio").HasMaxLength(200);
            entity.Property(x => x.NumeroConsultorio).HasColumnName("numero_consultorio").HasMaxLength(20);
            entity.Property(x => x.NumeroCadeiras).HasColumnName("numero_cadeiras").HasConversion<string>().IsRequired();
            entity.Property(x => x.OutrosEquipamentos).HasColumnName("outros_equipamentos").HasColumnType("TEXT");
            entity.Property(x => x.ObservacoesConsultorio).HasColumnName("observacoes_consultorio").HasColumnType("TEXT");

            // Horários
            entity.Property(x => x.SegundaSextaInicio).HasColumnName("segunda_sexta_inicio");
            entity.Property(x => x.SegundaSextaFim).HasColumnName("segunda_sexta_fim");
            entity.Property(x => x.SabadoInicio).HasColumnName("sabado_inicio");
            entity.Property(x => x.SabadoFim).HasColumnName("sabado_fim");
            entity.Property(x => x.DomingoInicio).HasColumnName("domingo_inicio");
            entity.Property(x => x.DomingoFim).HasColumnName("domingo_fim");
            entity.Property(x => x.TempoMedioConsulta).HasColumnName("tempo_medio_consulta").HasConversion<string>();

            // Dados Bancários
            entity.Property(x => x.Banco).HasColumnName("banco").IsRequired().HasMaxLength(10);
            entity.Property(x => x.TipoConta).HasColumnName("tipo_conta").HasConversion<string>().IsRequired();
            entity.Property(x => x.Agencia).HasColumnName("agencia").IsRequired().HasMaxLength(10);
            entity.Property(x => x.Conta).HasColumnName("conta").IsRequired().HasMaxLength(20);
            entity.Property(x => x.NomeTitular).HasColumnName("nome_titular").IsRequired().HasMaxLength(255);
            entity.Property(x => x.CpfTitular).HasColumnName("cpf_titular").IsRequired().HasMaxLength(14);

            // Acesso
            entity.Property(x => x.PerguntaSeguranca).HasColumnName("pergunta_seguranca").HasColumnType("TEXT");
            entity.Property(x => x.RespostaSegurancaHash).HasColumnName("resposta_seguranca_hash").HasMaxLength(255);

            // Termos
            entity.Property(x => x.TermosUso).HasColumnName("termos_uso").HasDefaultValue(false);
            entity.Property(x => x.CodigoEtica).HasColumnName("codigo_etica").HasDefaultValue(false);
            entity.Property(x => x.Responsabilidade).HasColumnName("responsabilidade").HasDefaultValue(false);
            entity.Property(x => x.DadosPessoais).HasColumnName("dados_pessoais").HasDefaultValue(false);
            entity.Property(x => x.Marketing).HasColumnName("marketing").HasDefaultValue(false);

            // Status
            entity.Property(x => x.StatusAprovacao).HasColumnName("status_aprovacao").HasConversion<string>().HasDefaultValue(StatusAprovacao.Pendente);
            entity.Property(x => x.Ativo).HasColumnName("ativo").HasDefaultValue(true);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Índices
            entity.HasIndex(x => x.Cpf).IsUnique();
            entity.HasIndex(x => x.Cro).IsUnique();
            entity.HasIndex(x => x.EmailProfissional).IsUnique();
            entity.HasIndex(x => x.StatusAprovacao);
        }
    }

    public class SolicitacaoOrcamentoMap : IEntityTypeConfiguration<SolicitacaoOrcamento>
    {
        public void Configure(EntityTypeBuilder<SolicitacaoOrcamento> entity)
        {
            entity.ToTable("solicitacoes_orcamento");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.NumeroPedido).HasColumnName("numero_pedido").IsRequired().HasMaxLength(50);
            entity.Property(x => x.PacienteId).HasColumnName("paciente_id");
            entity.Property(x => x.NomeCompleto).HasColumnName("nome_completo").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Cpf).HasColumnName("cpf").IsRequired().HasMaxLength(14);
            entity.Property(x => x.Telefone).HasColumnName("telefone").IsRequired().HasMaxLength(20);
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
            entity.Property(x => x.TipoTratamento).HasColumnName("tipo_tratamento").HasConversion<string>().IsRequired();
            entity.Property(x => x.Observacoes).HasColumnName("observacoes").HasColumnType("TEXT");
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(StatusSolicitacao.Pendente);
            entity.Property(x => x.ValorAprovado).HasColumnName("valor_aprovado").HasPrecision(10, 2);
            entity.Property(x => x.NumeroParcelas).HasColumnName("numero_parcelas");
            entity.Property(x => x.ValorParcela).HasColumnName("valor_parcela").HasPrecision(10, 2);
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Relacionamentos
            entity.HasOne(x => x.Paciente)
                .WithMany(x => x.SolicitacoesOrcamento)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Profissional)
                .WithMany(x => x.SolicitacoesOrcamento)
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.SetNull);

            // Índices
            entity.HasIndex(x => x.NumeroPedido).IsUnique();
            entity.HasIndex(x => x.Cpf);
            entity.HasIndex(x => x.Status);
        }
    }

    public class ProfissionalEspecialidadeMap : IEntityTypeConfiguration<ProfissionalEspecialidade>
    {
        public void Configure(EntityTypeBuilder<ProfissionalEspecialidade> entity)
        {
            entity.ToTable("profissional_especialidades");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id").IsRequired();
            entity.Property(x => x.Especialidade).HasColumnName("especialidade").HasConversion<string>().IsRequired();

            entity.HasOne(x => x.Profissional)
                .WithMany(x => x.Especialidades)
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProfissionalId, x.Especialidade }).IsUnique();
        }
    }

    public class ProfissionalEquipamentoMap : IEntityTypeConfiguration<ProfissionalEquipamento>
    {
        public void Configure(EntityTypeBuilder<ProfissionalEquipamento> entity)
        {
            entity.ToTable("profissional_equipamentos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id").IsRequired();
            entity.Property(x => x.Equipamento).HasColumnName("equipamento").HasConversion<string>().IsRequired();

            entity.HasOne(x => x.Profissional)
                .WithMany(x => x.Equipamentos)
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProfissionalId, x.Equipamento }).IsUnique();
        }
    }

    public class ProfissionalFacilidadeMap : IEntityTypeConfiguration<ProfissionalFacilidade>
    {
        public void Configure(EntityTypeBuilder<ProfissionalFacilidade> entity)
        {
            entity.ToTable("profissional_facilidades");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id").IsRequired();
            entity.Property(x => x.Facilidade).HasColumnName("facilidade").HasConversion<string>().IsRequired();

            entity.HasOne(x => x.Profissional)
                .WithMany(x => x.Facilidades)
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.ProfissionalId, x.Facilidade }).IsUnique();
        }
    }

    // Adicionar outros mappings conforme necessário...
    public class PlanejamentoDigitalMap : IEntityTypeConfiguration<PlanejamentoDigital>
    {
        public void Configure(EntityTypeBuilder<PlanejamentoDigital> entity)
        {
            entity.ToTable("planejamentos_digitais");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.SolicitacaoOrcamentoId).HasColumnName("solicitacao_orcamento_id");
            entity.Property(x => x.PacienteId).HasColumnName("paciente_id").IsRequired();
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id").IsRequired();
            entity.Property(x => x.NumeroAlinhadores).HasColumnName("numero_alinhadores").IsRequired();
            entity.Property(x => x.DuracaoTratamentoMeses).HasColumnName("duracao_tratamento_meses").IsRequired();
            entity.Property(x => x.Observacoes).HasColumnName("observacoes").HasColumnType("TEXT");
            entity.Property(x => x.OrcamentoEstimado).HasColumnName("orcamento_estimado").HasPrecision(10, 2).IsRequired();
            entity.Property(x => x.TipoAparelho).HasColumnName("tipo_aparelho").HasConversion<string>().IsRequired();
            entity.Property(x => x.PrioridadeCaso).HasColumnName("prioridade_caso").HasConversion<string>().HasDefaultValue(PrioridadeCaso.Normal);
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(StatusPlanejamento.Rascunho);
            entity.Property(x => x.DiasEntreTrocas).HasColumnName("dias_entre_trocas");
            entity.Property(x => x.ConsultasAcompanhamento).HasColumnName("consultas_acompanhamento");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Relacionamentos
            entity.HasOne(x => x.SolicitacaoOrcamento)
                .WithMany()
                .HasForeignKey(x => x.SolicitacaoOrcamentoId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Paciente)
                .WithMany(x => x.PlanejamentosDigitais)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Profissional)
                .WithMany()
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(x => x.Status);
            entity.HasIndex(x => x.TipoAparelho);
        }
    }

    public class AgendamentoMap : IEntityTypeConfiguration<Agendamento>
    {
        public void Configure(EntityTypeBuilder<Agendamento> entity)
        {
            entity.ToTable("agendamentos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.PacienteId).HasColumnName("paciente_id");
            entity.Property(x => x.ProfissionalId).HasColumnName("profissional_id").IsRequired();
            entity.Property(x => x.PacienteNome).HasColumnName("paciente_nome").IsRequired().HasMaxLength(255);
            entity.Property(x => x.DataAgendamento).HasColumnName("data_agendamento").IsRequired();
            entity.Property(x => x.HorarioInicio).HasColumnName("horario_inicio").IsRequired();
            entity.Property(x => x.DuracaoMinutos).HasColumnName("duracao_minutos").IsRequired().HasDefaultValue(60);
            entity.Property(x => x.Servico).HasColumnName("servico").HasConversion<string>().IsRequired();
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(StatusAgendamento.Agendado);
            entity.Property(x => x.Telefone).HasColumnName("telefone").HasMaxLength(20);
            entity.Property(x => x.Email).HasColumnName("email").HasMaxLength(255);
            entity.Property(x => x.Observacoes).HasColumnName("observacoes").HasColumnType("TEXT");
            entity.Property(x => x.ObservacoesCancelamento).HasColumnName("observacoes_cancelamento").HasColumnType("TEXT");
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");
            entity.Property(x => x.UpdatedAt).HasColumnName("updated_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Relacionamentos
            entity.HasOne(x => x.Paciente)
                .WithMany(x => x.Agendamentos)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(x => x.Profissional)
                .WithMany(x => x.Agendamentos)
                .HasForeignKey(x => x.ProfissionalId)
                .OnDelete(DeleteBehavior.Cascade);

            // Índices
            entity.HasIndex(x => x.DataAgendamento);
            entity.HasIndex(x => new { x.ProfissionalId, x.DataAgendamento });
            entity.HasIndex(x => x.Status);
        }
    }

    public class DocumentoMap : IEntityTypeConfiguration<Documento>
    {
        public void Configure(EntityTypeBuilder<Documento> entity)
        {
            entity.ToTable("documentos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.EntidadeTipo).HasColumnName("entidade_tipo").HasConversion<string>().IsRequired();
            entity.Property(x => x.EntidadeId).HasColumnName("entidade_id").IsRequired();
            entity.Property(x => x.TipoDocumento).HasColumnName("tipo_documento").HasConversion<string>().IsRequired();
            entity.Property(x => x.NomeOriginal).HasColumnName("nome_original").IsRequired().HasMaxLength(255);
            entity.Property(x => x.NomeArquivo).HasColumnName("nome_arquivo").IsRequired().HasMaxLength(255);
            entity.Property(x => x.CaminhoArquivo).HasColumnName("caminho_arquivo").IsRequired().HasMaxLength(500);
            entity.Property(x => x.TamanhoBytes).HasColumnName("tamanho_bytes").IsRequired();
            entity.Property(x => x.TipoMime).HasColumnName("tipo_mime").IsRequired().HasMaxLength(100);
            entity.Property(x => x.Status).HasColumnName("status").HasConversion<string>().HasDefaultValue(StatusDocumento.Pendente);
            entity.Property(x => x.CreatedAt).HasColumnName("created_at").HasDefaultValueSql("CURRENT_TIMESTAMP(6)");

            // Índices
            entity.HasIndex(x => new { x.EntidadeTipo, x.EntidadeId });
            entity.HasIndex(x => x.TipoDocumento);
        }
    }

    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> entity)
        {
            entity.ToTable("users");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Password).HasColumnName("password").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(x => x.PhoneNumber).HasColumnName("phone_number").HasMaxLength(20);
            entity.Property(x => x.UserType).HasColumnName("user_type").HasConversion<string>().IsRequired();
            entity.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);

            entity.HasIndex(x => x.Email).IsUnique();
        }
    }

    public class CompanyMap : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> entity)
        {
            entity.ToTable("companies");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.CompanyName).HasColumnName("company_name").IsRequired().HasMaxLength(255);
            entity.Property(x => x.TradingName).HasColumnName("trading_name").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Cnpj).HasColumnName("cnpj").IsRequired().HasMaxLength(18);
            entity.Property(x => x.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
            entity.Property(x => x.PhoneNumber).HasColumnName("phone_number").IsRequired().HasMaxLength(20);
            entity.Property(x => x.Contact).HasColumnName("contact").IsRequired().HasMaxLength(255);
            entity.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);

            entity.HasIndex(x => x.Cnpj).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
        }
    }

    public class BankMap : IEntityTypeConfiguration<Bank>
    {
        public void Configure(EntityTypeBuilder<Bank> entity)
        {
            entity.ToTable("banks");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
            entity.Property(x => x.BankNumber).HasColumnName("bank_number").IsRequired();
            entity.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);

            entity.HasIndex(x => x.BankNumber).IsUnique();
        }
    }

    public class BankAccountMap : IEntityTypeConfiguration<BankAccount>
    {
        public void Configure(EntityTypeBuilder<BankAccount> entity)
        {
            entity.ToTable("bank_accounts");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Id).HasColumnName("id").ValueGeneratedNever();
            entity.Property(x => x.AccountNumber).HasColumnName("account_number").IsRequired().HasMaxLength(20);
            entity.Property(x => x.AccountDigit).HasColumnName("account_digit").HasMaxLength(2);
            entity.Property(x => x.Agency).HasColumnName("agency").HasMaxLength(10);
            entity.Property(x => x.AccountType).HasColumnName("account_type").HasConversion<string>().IsRequired();
            entity.Property(x => x.Active).HasColumnName("active").HasDefaultValue(true);
            entity.Property(x => x.BankId).HasColumnName("bank_id").IsRequired();

            entity.HasOne(x => x.Bank)
                .WithMany()
                .HasForeignKey(x => x.BankId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(x => new { x.BankId, x.AccountNumber, x.AccountDigit }).IsUnique();
        }
    }
}
