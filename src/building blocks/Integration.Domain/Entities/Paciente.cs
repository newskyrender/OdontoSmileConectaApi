using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class Paciente : Entity
    {
        protected Paciente() { }

        public Paciente(Guid id, string nomeCompleto, string cpf, DateTime dataNascimento,
            EstadoCivil estadoCivil, Sexo sexo, string celularPrincipal, string email,
            string cep, string enderecoCompleto, string bairro, string cidade, string estado)
        {
            if (id != Guid.Empty) Id = id;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            DataNascimento = dataNascimento;
            EstadoCivil = estadoCivil;
            Sexo = sexo;
            CelularPrincipal = celularPrincipal;
            Email = email;
            Cep = cep;
            EnderecoCompleto = enderecoCompleto;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Status = StatusPaciente.Ativo;

            new ValidationContract<Paciente>(this)
                .IsRequired(x => x.NomeCompleto, "O nome completo deve ser informado")
                .IsRequired(x => x.Cpf, "O CPF deve ser informado")
                .IsRequired(x => x.CelularPrincipal, "O celular principal deve ser informado")
                .IsRequired(x => x.Email, "O e-mail deve ser informado")
                .IsEmail(x => x.Email, "O e-mail informado deve ser válido")
                .IsRequired(x => x.Cep, "O CEP deve ser informado")
                .IsRequired(x => x.EnderecoCompleto, "O endereço completo deve ser informado")
                .IsRequired(x => x.Bairro, "O bairro deve ser informado")
                .IsRequired(x => x.Cidade, "A cidade deve ser informada")
                .IsRequired(x => x.Estado, "O estado deve ser informado");
        }

        // Dados Pessoais
        public string NomeCompleto { get; private set; }
        public string Cpf { get; private set; }
        public DateTime DataNascimento { get; private set; }
        public EstadoCivil EstadoCivil { get; private set; }
        public Sexo Sexo { get; private set; }
        public string Profissao { get; private set; }

        // Dados de Contato
        public string CelularPrincipal { get; private set; }
        public string TelefoneFixo { get; private set; }
        public string Email { get; private set; }
        public string ContatoEmergencia { get; private set; }

        // Endereço
        public string Cep { get; private set; }
        public string EnderecoCompleto { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public string Complemento { get; private set; }
        public string Numero { get; private set; }

        // Dados COOP1000
        public string NumeroCooperado { get; private set; }
        public string SituacaoCooperativa { get; private set; }
        public decimal? RendaMensal { get; private set; }
        public decimal? LimiteDisponivel { get; private set; }
        public decimal? LimiteTotal { get; private set; }
        public decimal LimiteUtilizado { get; private set; } = 0;

        // Controle
        public StatusPaciente Status { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual ICollection<SolicitacaoOrcamento> SolicitacoesOrcamento { get; private set; } = new List<SolicitacaoOrcamento>();
        public virtual ICollection<Agendamento> Agendamentos { get; private set; } = new List<Agendamento>();
        public virtual ICollection<PlanejamentoDigital> PlanejamentosDigitais { get; private set; } = new List<PlanejamentoDigital>();

        // Methods
        public void AtualizarDadosPessoais(string nomeCompleto, DateTime dataNascimento,
            EstadoCivil estadoCivil, Sexo sexo, string profissao)
        {
            NomeCompleto = nomeCompleto;
            DataNascimento = dataNascimento;
            EstadoCivil = estadoCivil;
            Sexo = sexo;
            Profissao = profissao;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarContato(string celularPrincipal, string telefoneFixo,
            string email, string contatoEmergencia)
        {
            CelularPrincipal = celularPrincipal;
            TelefoneFixo = telefoneFixo;
            Email = email;
            ContatoEmergencia = contatoEmergencia;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarEndereco(string cep, string enderecoCompleto, string bairro,
            string cidade, string estado, string complemento, string numero)
        {
            Cep = cep;
            EnderecoCompleto = enderecoCompleto;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            Complemento = complemento;
            Numero = numero;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarDadosCooperativa(string numeroCooperado, string situacaoCooperativa,
            decimal? rendaMensal, decimal? limiteDisponivel, decimal? limiteTotal)
        {
            NumeroCooperado = numeroCooperado;
            SituacaoCooperativa = situacaoCooperativa;
            RendaMensal = rendaMensal;
            LimiteDisponivel = limiteDisponivel;
            LimiteTotal = limiteTotal;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AtualizarLimiteUtilizado(decimal valor)
        {
            LimiteUtilizado = valor;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AlterarStatus(StatusPaciente novoStatus)
        {
            Status = novoStatus;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}