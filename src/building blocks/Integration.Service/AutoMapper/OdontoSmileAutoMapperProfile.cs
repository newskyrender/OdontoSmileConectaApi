using AutoMapper;
using Integration.Domain.Entities;
using Integration.Domain.Http.Request;
using Integration.Domain.Http.Response;
using Integration.Domain.Enums;

namespace Integration.Service.AutoMapper
{
    public class OdontoSmileAutoMapperProfile : Profile
    {
        public OdontoSmileAutoMapperProfile()
        {
            CreateMap<Usuario, UsuarioResponse>();
            CreateMap<UsuarioRegisterRequest, Usuario>()
                .ConstructUsing(src => new Usuario(default, src.Email, "", src.Nome, src.Ativo));

            CreateMap<Paciente, PacienteResponse>();
            CreateMap<PacienteRegisterRequest, Paciente>()
                .ConstructUsing(src => new Paciente(default, src.NomeCompleto, src.Cpf, src.DataNascimento,
                    src.EstadoCivil, src.Sexo, src.CelularPrincipal, src.Email, src.Cep, src.EnderecoCompleto,
                    src.Bairro, src.Cidade, src.Estado));

            CreateMap<Profissional, ProfissionalResponse>()
                .ForMember(dest => dest.Especialidades, opt => opt.MapFrom(src => src.Especialidades.Select(e => e.Especialidade)))
                .ForMember(dest => dest.Equipamentos, opt => opt.MapFrom(src => src.Equipamentos.Select(e => e.Equipamento)))
                .ForMember(dest => dest.Facilidades, opt => opt.MapFrom(src => src.Facilidades.Select(f => f.Facilidade)));

            CreateMap<ProfissionalRegisterRequest, Profissional>()
                .ConstructUsing(src => new Profissional(default, src.NomeCompleto, src.Cpf, src.DataNascimento,
                    src.Sexo, src.EmailProfissional, src.Celular, src.Cro, src.DataFormatura,
                    src.UniversidadeFormacao, src.TempoExperiencia));

            CreateMap<SolicitacaoOrcamento, SolicitacaoOrcamentoResponse>()
                .ForMember(dest => dest.ProfissionalNome, opt => opt.MapFrom(src => src.Profissional != null ? src.Profissional.NomeCompleto : ""));

            CreateMap<SolicitacaoOrcamentoRegisterRequest, SolicitacaoOrcamento>()
                .ConstructUsing(src => new SolicitacaoOrcamento(default, "", src.NomeCompleto, src.Cpf, src.Telefone, src.TipoTratamento));

            CreateMap<PlanejamentoDigital, PlanejamentoDigitalResponse>()
                .ForMember(dest => dest.PacienteNome, opt => opt.MapFrom(src => src.Paciente != null ? src.Paciente.NomeCompleto : ""))
                .ForMember(dest => dest.ProfissionalNome, opt => opt.MapFrom(src => src.Profissional != null ? src.Profissional.NomeCompleto : ""));

            CreateMap<PlanejamentoDigitalRegisterRequest, PlanejamentoDigital>()
                .ConstructUsing(src => new PlanejamentoDigital(default, src.PacienteId, src.ProfissionalId,
                    src.NumeroAlinhadores, src.DuracaoTratamentoMeses, src.OrcamentoEstimado, src.TipoAparelho));

            CreateMap<Agendamento, AgendamentoResponse>()
                .ForMember(dest => dest.ProfissionalNome, opt => opt.MapFrom(src => src.Profissional != null ? src.Profissional.NomeCompleto : ""));

            CreateMap<AgendamentoRegisterRequest, Agendamento>()
                .ConstructUsing(src => new Agendamento(default, src.ProfissionalId, src.PacienteNome,
                    src.DataAgendamento, src.HorarioInicio, src.Servico));

            CreateMap<Documento, DocumentoResponse>();
            CreateMap<DocumentoUploadRequest, Documento>()
                .ConstructUsing(src => new Documento(default, src.EntidadeTipo, src.EntidadeId, src.TipoDocumento,
                    src.NomeOriginal, src.NomeArquivo, src.CaminhoArquivo, src.TamanhoBytes, src.TipoMime));

            // Mapeamentos para enums
            CreateMap<EstadoCivil, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, EstadoCivil>().ConvertUsing(src => Enum.Parse<EstadoCivil>(src, true));

            CreateMap<Sexo, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, Sexo>().ConvertUsing(src => Enum.Parse<Sexo>(src, true));

            CreateMap<StatusPaciente, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusPaciente>().ConvertUsing(src => Enum.Parse<StatusPaciente>(src, true));

            CreateMap<StatusAprovacao, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusAprovacao>().ConvertUsing(src => Enum.Parse<StatusAprovacao>(src, true));

            CreateMap<TempoExperiencia, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TempoExperiencia>().ConvertUsing(src => Enum.Parse<TempoExperiencia>(src, true));

            CreateMap<NumeroCadeiras, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, NumeroCadeiras>().ConvertUsing(src => Enum.Parse<NumeroCadeiras>(src, true));

            CreateMap<TipoConta, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoConta>().ConvertUsing(src => Enum.Parse<TipoConta>(src, true));

            CreateMap<TipoTratamento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoTratamento>().ConvertUsing(src => Enum.Parse<TipoTratamento>(src, true));

            CreateMap<StatusSolicitacao, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusSolicitacao>().ConvertUsing(src => Enum.Parse<StatusSolicitacao>(src, true));

            CreateMap<TipoAparelho, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoAparelho>().ConvertUsing(src => Enum.Parse<TipoAparelho>(src, true));

            CreateMap<PrioridadeCaso, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, PrioridadeCaso>().ConvertUsing(src => Enum.Parse<PrioridadeCaso>(src, true));

            CreateMap<StatusPlanejamento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusPlanejamento>().ConvertUsing(src => Enum.Parse<StatusPlanejamento>(src, true));

            CreateMap<ServicoAgendamento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, ServicoAgendamento>().ConvertUsing(src => Enum.Parse<ServicoAgendamento>(src, true));

            CreateMap<StatusAgendamento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusAgendamento>().ConvertUsing(src => Enum.Parse<StatusAgendamento>(src, true));

            CreateMap<TipoDocumento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoDocumento>().ConvertUsing(src => Enum.Parse<TipoDocumento>(src, true));

            CreateMap<StatusDocumento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusDocumento>().ConvertUsing(src => Enum.Parse<StatusDocumento>(src, true));

            CreateMap<EntidadeTipo, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, EntidadeTipo>().ConvertUsing(src => Enum.Parse<EntidadeTipo>(src, true));

            CreateMap<Especialidade, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, Especialidade>().ConvertUsing(src => Enum.Parse<Especialidade>(src, true));

            CreateMap<Equipamento, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, Equipamento>().ConvertUsing(src => Enum.Parse<Equipamento>(src, true));

            CreateMap<Facilidade, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, Facilidade>().ConvertUsing(src => Enum.Parse<Facilidade>(src, true));

            CreateMap<TipoTransacao, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoTransacao>().ConvertUsing(src => Enum.Parse<TipoTransacao>(src, true));

            CreateMap<StatusTransacao, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusTransacao>().ConvertUsing(src => Enum.Parse<StatusTransacao>(src, true));

            CreateMap<ResultadoAnaliseCredito, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, ResultadoAnaliseCredito>().ConvertUsing(src => Enum.Parse<ResultadoAnaliseCredito>(src, true));

            CreateMap<TipoAnalise, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, TipoAnalise>().ConvertUsing(src => Enum.Parse<TipoAnalise>(src, true));

            CreateMap<StatusInadimplencia, string>().ConvertUsing(src => src.ToString().ToLower());
            CreateMap<string, StatusInadimplencia>().ConvertUsing(src => Enum.Parse<StatusInadimplencia>(src, true));

            // Mapeamentos específicos para TimeSpan
            CreateMap<TimeSpan?, string>().ConvertUsing(src => src.HasValue ? src.Value.ToString(@"hh\:mm") : "");
            CreateMap<string, TimeSpan?>().ConvertUsing(src =>
                string.IsNullOrEmpty(src) ? (TimeSpan?)null : TimeSpan.Parse(src));

            CreateMap<TimeSpan, string>().ConvertUsing(src => src.ToString(@"hh\:mm"));
            CreateMap<string, TimeSpan>().ConvertUsing(src => TimeSpan.Parse(src));

            // Mapeamentos específicos para TempoConsulta enum
            CreateMap<TempoConsulta?, string>().ConvertUsing(src =>
                src != null ? src.ToString().Replace("Minutos", "_min").ToLower() : "");
            CreateMap<string, TempoConsulta?>().ConvertUsing(src =>
                string.IsNullOrEmpty(src) ? (TempoConsulta?)null :
                Enum.Parse<TempoConsulta>(src.Replace("_min", "Minutos"), true));
        }
    }
}
