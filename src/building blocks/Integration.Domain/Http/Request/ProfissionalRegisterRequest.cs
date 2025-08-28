using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // Profissional Requests
    public class ProfissionalRegisterRequest : ICommand
    {
        // Dados Pessoais
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public string EmailProfissional { get; set; }
        public string Celular { get; set; }
        public string TelefoneAdicional { get; set; }

        // Dados Profissionais
        public string Cro { get; set; }
        public DateTime DataFormatura { get; set; }
        public string UniversidadeFormacao { get; set; }
        public TempoExperiencia TempoExperiencia { get; set; }
        public string OutrasEspecialidades { get; set; }
        public List<Especialidade> Especialidades { get; set; } = new List<Especialidade>();

        // Dados do Consultório
        public string NomeConsultorio { get; set; }
        public string Cnpj { get; set; }
        public string TelefoneConsultorio { get; set; }
        public string CepConsultorio { get; set; }
        public string EnderecoConsultorio { get; set; }
        public string BairroConsultorio { get; set; }
        public string CidadeConsultorio { get; set; }
        public string EstadoConsultorio { get; set; }
        public string ComplementoConsultorio { get; set; }
        public string NumeroConsultorio { get; set; }
        public NumeroCadeiras NumeroCadeiras { get; set; }
        public string OutrosEquipamentos { get; set; }
        public string ObservacoesConsultorio { get; set; }
        public List<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
        public List<Facilidade> Facilidades { get; set; } = new List<Facilidade>();

        // Horários
        public TimeSpan? SegundaSextaInicio { get; set; }
        public TimeSpan? SegundaSextaFim { get; set; }
        public TimeSpan? SabadoInicio { get; set; }
        public TimeSpan? SabadoFim { get; set; }
        public TimeSpan? DomingoInicio { get; set; }
        public TimeSpan? DomingoFim { get; set; }
        public TempoConsulta? TempoMedioConsulta { get; set; }

        // Dados Bancários
        public string Banco { get; set; }
        public TipoConta? TipoConta { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeTitular { get; set; }
        public string CpfTitular { get; set; }

        // Termos
        public bool TermosUso { get; set; }
        public bool CodigoEtica { get; set; }
        public bool Responsabilidade { get; set; }
        public bool DadosPessoais { get; set; }
        public bool Marketing { get; set; }
    }
}
