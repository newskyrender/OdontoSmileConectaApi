using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Request
{
    // Paciente Requests
    public class PacienteRegisterRequest : ICommand
    {
        public string NomeCompleto { get; set; }
        public string Cpf { get; set; }
        public DateTime DataNascimento { get; set; }
        public EstadoCivil EstadoCivil { get; set; }
        public Sexo Sexo { get; set; }
        public string Profissao { get; set; }
        public string CelularPrincipal { get; set; }
        public string TelefoneFixo { get; set; }
        public string Email { get; set; }
        public string ContatoEmergencia { get; set; }
        public string Cep { get; set; }
        public string EnderecoCompleto { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Complemento { get; set; }
        public string Numero { get; set; }
        public string NumeroCooperado { get; set; }
        public string SituacaoCooperativa { get; set; }
        public decimal? RendaMensal { get; set; }
        public decimal? LimiteDisponivel { get; set; }
        public decimal? LimiteTotal { get; set; }
    }
}
