using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class Profissional : Entity
    {
        protected Profissional() { }

        public Profissional(Guid id, string nomeCompleto, string cpf, DateTime dataNascimento,
            Sexo sexo, string emailProfissional, string celular, string cro, DateTime dataFormatura,
            string universidadeFormacao, TempoExperiencia tempoExperiencia)
        {
            if (id != Guid.Empty) Id = id;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            Sexo = sexo;
            EmailProfissional = emailProfissional;
            Celular = celular;
            Cro = cro;
            DataFormatura = dataFormatura;
            UniversidadeFormacao = universidadeFormacao;
            TempoExperiencia = tempoExperiencia;
            StatusAprovacao = StatusAprovacao.Pendente;
            Ativo = true;

            new ValidationContract<Profissional>(this)
                .IsRequired(x => x.NomeCompleto, "O nome completo deve ser informado")
                .IsRequired(x => x.Cpf, "O CPF deve ser informado")
                .IsRequired(x => x.EmailProfissional, "O e-mail profissional deve ser informado")
                .IsEmail(x => x.EmailProfissional, "O e-mail informado deve ser válido")
                .IsRequired(x => x.Celular, "O celular deve ser informado")
                .IsRequired(x => x.Cro, "O CRO deve ser informado")
                .IsRequired(x => x.UniversidadeFormacao, "A universidade de formação deve ser informada");
        }

        // Dados Pessoais
        public string NomeCompleto { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public Sexo Sexo { get; private set; }
        public string EmailProfissional { get; private set; }
        public string Celular { get; private set; }
        public string TelefoneAdicional { get; private set; }

        // Dados Profissionais
        public string Cro { get; private set; }
        public DateTime DataFormatura { get; private set; }
        public string UniversidadeFormacao { get; private set; }
        public TempoExperiencia TempoExperiencia { get; private set; }
        public string OutrasEspecialidades { get; private set; }

        // Dados do Consultório
        public string NomeConsultorio { get; private set; }
        public string Cnpj { get; private set; }
        public string TelefoneConsultorio { get; private set; }
        public string CepConsultorio { get; private set; }
        public string EnderecoConsultorio { get; private set; }
        public string BairroConsultorio { get; private set; }
        public string CidadeConsultorio { get; private set; }
        public string EstadoConsultorio { get; private set; }
        public string ComplementoConsultorio { get; private set; }
        public string NumeroConsultorio { get; private set; }
        public NumeroCadeiras NumeroCadeiras { get; private set; }
        public string OutrosEquipamentos { get; private set; }
        public string ObservacoesConsultorio { get; private set; }

        // Horários de Funcionamento
        public TimeSpan? SegundaSextaInicio { get; private set; }
        public TimeSpan? SegundaSextaFim { get; private set; }
        public TimeSpan? SabadoInicio { get; private set; }
        public TimeSpan? SabadoFim { get; private set; }
        public TimeSpan? DomingoInicio { get; private set; }
        public TimeSpan? DomingoFim { get; private set; }
        public TempoConsulta? TempoMedioConsulta { get; private set; }

        // Dados Bancários
        public string Banco { get; private set; }
        public TipoConta? TipoConta { get; private set; }
        public string Agencia { get; private set; }
        public string Conta { get; private set; }
        public string NomeTitular { get; private set; }
        public string CpfTitular { get; private set; }

        // Acesso ao Sistema
        public string PerguntaSeguranca { get; private set; }
        public string RespostaSegurancaHash { get; private set; }

        // Termos de Aceite
        public bool TermosUso { get; private set; }
        public bool CodigoEtica { get; private set; }
        public bool Responsabilidade { get; private set; }
        public bool DadosPessoais { get; private set; }
        public bool Marketing { get; private set; }

        // Status
        public StatusAprovacao StatusAprovacao { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<ProfissionalEspecialidade> Especialidades { get; private set; } = new List<ProfissionalEspecialidade>();
        public virtual ICollection<ProfissionalEquipamento> Equipamentos { get; private set; } = new List<ProfissionalEquipamento>();
        public virtual ICollection<ProfissionalFacilidade> Facilidades { get; private set; } = new List<ProfissionalFacilidade>();
        public virtual ICollection<SolicitacaoOrcamento> SolicitacoesOrcamento { get; private set; } = new List<SolicitacaoOrcamento>();
        public virtual ICollection<Agendamento> Agendamentos { get; private set; } = new List<Agendamento>();

        // Methods
        public void AtualizarDadosConsultorio(string nomeConsultorio, string cnpj, string telefoneConsultorio,
            string cepConsultorio, string enderecoConsultorio, string bairroConsultorio, string cidadeConsultorio,
            string estadoConsultorio, string complementoConsultorio, string numeroConsultorio,
            NumeroCadeiras numeroCadeiras)
        {
            NomeConsultorio = nomeConsultorio;
            Cnpj = cnpj;
            TelefoneConsultorio = telefoneConsultorio;
            CepConsultorio = cepConsultorio;
            EnderecoConsultorio = enderecoConsultorio;
            BairroConsultorio = bairroConsultorio;
            CidadeConsultorio = cidadeConsultorio;
            EstadoConsultorio = estadoConsultorio;
            ComplementoConsultorio = complementoConsultorio;
            NumeroConsultorio = numeroConsultorio;
            NumeroCadeiras = numeroCadeiras;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarHorariosFuncionamento(TimeSpan? segundaSextaInicio, TimeSpan? segundaSextaFim,
            TimeSpan? sabadoInicio, TimeSpan? sabadoFim, TimeSpan? domingoInicio, TimeSpan? domingoFim,
            TempoConsulta? tempoMedioConsulta)
        {
            SegundaSextaInicio = segundaSextaInicio;
            SegundaSextaFim = segundaSextaFim;
            SabadoInicio = sabadoInicio;
            SabadoFim = sabadoFim;
            DomingoInicio = domingoInicio;
            DomingoFim = domingoFim;
            TempoMedioConsulta = tempoMedioConsulta;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarDadosBancarios(string banco, TipoConta tipoConta, string agencia,
            string conta, string nomeTitular, string cpfTitular)
        {
            Banco = banco;
            TipoConta = tipoConta;
            Agencia = agencia;
            Conta = conta;
            NomeTitular = nomeTitular;
            CpfTitular = cpfTitular;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AceitarTermos(bool termosUso, bool codigoEtica, bool responsabilidade,
            bool dadosPessoais, bool marketing)
        {
            TermosUso = termosUso;
            CodigoEtica = codigoEtica;
            Responsabilidade = responsabilidade;
            DadosPessoais = dadosPessoais;
            Marketing = marketing;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AlterarStatusAprovacao(StatusAprovacao novoStatus)
        {
            StatusAprovacao = novoStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }

    public class ProfissionalEspecialidade : Entity
    {
        protected ProfissionalEspecialidade() { }

        public ProfissionalEspecialidade(Guid profissionalId, Especialidade especialidade)
        {
            ProfissionalId = profissionalId;
            Especialidade = especialidade;
        }

        public Guid ProfissionalId { get; private set; }
        public Especialidade Especialidade { get; private set; }

        public virtual Profissional Profissional { get; private set; }
    }

    public class ProfissionalEquipamento : Entity
    {
        protected ProfissionalEquipamento() { }

        public ProfissionalEquipamento(Guid profissionalId, Equipamento equipamento)
        {
            ProfissionalId = profissionalId;
            Equipamento = equipamento;
        }

        public Guid ProfissionalId { get; private set; }
        public Equipamento Equipamento { get; private set; }

        public virtual Profissional Profissional { get; private set; }
    }

    public class ProfissionalFacilidade : Entity
    {
        protected ProfissionalFacilidade() { }

        public ProfissionalFacilidade(Guid profissionalId, Facilidade facilidade)
        {
            ProfissionalId = profissionalId;
            Facilidade = facilidade;
        }

        public Guid ProfissionalId { get; private set; }
        public Facilidade Facilidade { get; private set; }

        public virtual Profissional Profissional { get; private set; }
    }
}