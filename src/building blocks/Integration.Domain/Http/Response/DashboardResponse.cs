using Integration.Domain.Enums;
using Integration.Domain.Common;

namespace Integration.Domain.Http.Response
{
    public class DashboardTotaisResponse : ICommandResult
    {
        public int TotalPacientes { get; set; }
        public int ConsultasHoje { get; set; }
    }

    public class ConsultaHojeResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string NomePaciente { get; set; }
        public string NomeProfissional { get; set; }
        public DateTime DataConsulta { get; set; }
        public string Status { get; set; }
        public string TipoConsulta { get; set; }
        public string Observacoes { get; set; }
    }

    public class StatusRapidoResponse : ICommandResult
    {
        public ConsultasConcluidasInfo ConsultasConcluidas { get; set; }
        public PendenciasInfo Pendencias { get; set; }
        public CancelamentosInfo Cancelamentos { get; set; }
    }

    public class ConsultasConcluidasInfo
    {
        public int Concluidas { get; set; }
        public int TotalHoje { get; set; }
        public string Descricao => $"{Concluidas} de {TotalHoje} consultas hoje";
    }

    public class PendenciasInfo
    {
        public int Quantidade { get; set; }
        public string Descricao => $"{Quantidade} consultas aguardando";
    }

    public class CancelamentosInfo
    {
        public int Quantidade { get; set; }
        public string Descricao => $"{Quantidade} cancelamentos esta semana";
    }

    public class PacienteRecenteResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public DateTime? DataUltimaConsulta { get; set; }
        public DateTime? DataProximaConsulta { get; set; }
        public string Status { get; set; } // "Ativo" ou "Inativo"
        public string StatusClass => Status == "Ativo" ? "success" : "secondary";
    }
}
