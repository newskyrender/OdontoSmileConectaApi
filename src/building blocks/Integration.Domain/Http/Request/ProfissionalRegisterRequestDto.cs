using Integration.Domain.Common;
using System.Text.Json.Serialization;

namespace Integration.Domain.Http.Request
{
    /// <summary>
    /// DTO para receber os dados de cadastro do profissional no formato JSON específico
    /// </summary>
    public class ProfissionalRegisterRequestDto : ICommand
    {
        // Dados Pessoais
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public string Sexo { get; set; }
        public string EmailProfissional { get; set; }
        public string Celular { get; set; }
        public string TelefoneAdicional { get; set; }

        // Dados Profissionais
        public string Cro { get; set; }
        public DateTime DataFormatura { get; set; }
        public string UniversidadeFormacao { get; set; }
        public string TempoExperiencia { get; set; }
        public string OutrasEspecialidades { get; set; }
        public List<string> Especialidades { get; set; } = new List<string>();

        // Dados do Consultório
        public string NomeConsultorio { get; set; }
        public string Cnpj { get; set; }
        public string TelefoneConsultorio { get; set; }
        public string Cep { get; set; }
        public string EnderecoCompleto { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string NumeroCadeiras { get; set; }
        public string OutrosEquipamentos { get; set; }
        public string ObservacoesConsultorio { get; set; }
        public List<string> Equipamentos { get; set; } = new List<string>();
        public List<string> Facilidades { get; set; } = new List<string>();

        // Horários de Funcionamento (objeto aninhado)
        public HorarioFuncionamentoDto HorarioFuncionamento { get; set; }
        public string TempoMedioConsulta { get; set; }

        // Dados Bancários (objeto aninhado)
        public DadosBancariosDto DadosBancarios { get; set; }

        // Acesso ao Sistema
        public string Senha { get; set; }
        public string PerguntaSeguranca { get; set; }
        public string RespostaSeguranca { get; set; }

        // Termos de Aceite (objeto aninhado)
        public TermosAceitosDto TermosAceitos { get; set; }
    }

    public class HorarioFuncionamentoDto
    {
        public HorarioPeriodoDto SegundaSexta { get; set; }
        public HorarioPeriodoDto Sabado { get; set; }
        public HorarioPeriodoDto Domingo { get; set; }
    }

    public class HorarioPeriodoDto
    {
        public string Inicio { get; set; }
        public string Fim { get; set; }
    }

    public class DadosBancariosDto
    {
        public string Banco { get; set; }
        public string TipoConta { get; set; }
        public string Agencia { get; set; }
        public string Conta { get; set; }
        public string NomeTitular { get; set; }
        public string CpfTitular { get; set; }
    }

    public class TermosAceitosDto
    {
        public bool TermosUso { get; set; }
        public bool CodigoEtica { get; set; }
        public bool Responsabilidade { get; set; }
        public bool DadosPessoais { get; set; }
        public bool Marketing { get; set; }
    }
}
