using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Http.Response
{
    // Profissional Responses
    public class ProfissionalResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public Sexo Sexo { get; set; }
        public string EmailProfissional { get; set; }
        public string Celular { get; set; }
        public string TelefoneAdicional { get; set; }
        public string Cro { get; set; }
        public DateTime DataFormatura { get; set; }
        public string UniversidadeFormacao { get; set; }
        public TempoExperiencia TempoExperiencia { get; set; }
        public string OutrasEspecialidades { get; set; }
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
        public TimeSpan? SegundaSextaInicio { get; set; }
        public TimeSpan? SegundaSextaFim { get; set; }
        public TimeSpan? SabadoInicio { get; set; }
        public TimeSpan? SabadoFim { get; set; }
        public TimeSpan? DomingoInicio { get; set; }
        public TimeSpan? DomingoFim { get; set; }
        public TempoConsulta? TempoMedioConsulta { get; set; }
        public string Banco { get; set; }
        public TipoConta? TipoConta { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeTitular { get; set; }
        public string CpfTitular { get; set; }
        public bool TermosUso { get; set; }
        public bool CodigoEtica { get; set; }
        public bool Responsabilidade { get; set; }
        public bool DadosPessoais { get; set; }
        public bool Marketing { get; set; }
        public StatusAprovacao StatusAprovacao { get; set; }
        public bool Ativo { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public List<Especialidade> Especialidades { get; set; } = new List<Especialidade>();
        public List<Equipamento> Equipamentos { get; set; } = new List<Equipamento>();
        public List<Facilidade> Facilidades { get; set; } = new List<Facilidade>();
    }
}
