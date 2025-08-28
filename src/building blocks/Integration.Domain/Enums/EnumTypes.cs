using System.ComponentModel;

namespace Integration.Domain.Enums
{
    public enum UserType
    {
        [Description("Administrador")]
        Administrator = 1,
        [Description("Usuário")]
        User = 2,
    }

    public enum AccountType
    {
        [Description("Conta Corrente")]
        CheckingAccount = 1,
        [Description("Conta Poupanca")]
        DepositAccount = 2,
    }

    public enum StatusCodeType
    {
        [Description("Recebida")]
        Approved = 1,
        [Description("Pendente")]
        Pending = 2,
        [Description("Cancelada")]
        Canceled = 3,
        [Description("Erro")]
        Error = 4
    }

    public enum EstadoCivil
    {
        Solteiro = 1,
        Casado = 2,
        Divorciado = 3,
        Viuvo = 4,
        Outro = 5
    }

    public enum Sexo
    {
        Masculino = 1,
        Feminino = 2,
        Outro = 3,
        NaoInformar = 4
    }

    public enum StatusPaciente
    {
        Ativo = 1,
        Inativo = 2,
        Suspenso = 3
    }

    public enum StatusAprovacao
    {
        Pendente = 1,
        Aprovado = 2,
        Rejeitado = 3,
        EmAnalise = 4
    }

    public enum TempoExperiencia
    {
        Menos1Ano = 1,
        Entre1e5Anos = 2,
        Entre6e10Anos = 3,
        Entre11e20Anos = 4,
        Mais20Anos = 5
    }

    public enum NumeroCadeiras
    {
        UmaCadeira = 1,
        DuasCadeiras = 2,
        TresCadeiras = 3,
        QuatroCadeiras = 4,
        CincoOuMaisCadeiras = 5
    }

    public enum TipoConta
    {
        ContaCorrente = 1,
        ContaPoupanca = 2
    }

    public enum TempoConsulta
    {
        TrintaMinutos = 30,
        QuarentaCincoMinutos = 45,
        SessentaMinutos = 60,
        NoventaMinutos = 90,
        CentoVinteMinutos = 120
    }

    public enum Especialidade
    {
        Ortodontia = 1,
        Implantodontia = 2,
        Endodontia = 3,
        Protese = 4,
        Periodontia = 5,
        CirurgiaOral = 6,
        Dentistica = 7,
        ClinicaGeral = 8
    }

    public enum Equipamento
    {
        ScannerItero = 1,
        ScannerMedit = 2,
        Scanner3Shape = 3,
        ScannerCerec = 4,
        RaioXDigital = 5,
        Panoramica = 6,
        Tomografia = 7,
        LaserTerapeutico = 8
    }

    public enum Facilidade
    {
        Estacionamento = 1,
        Acessibilidade = 2,
        ArCondicionado = 3,
        Wifi = 4
    }

    public enum TipoTratamento
    {
        Invisalign = 1,
        CorretorDireto = 2,
        AparelhoConvencional = 3,
        Limpeza = 4,
        Extracao = 5,
        Implante = 6,
        Protese = 7,
        Clareamento = 8,
        Outro = 9
    }

    public enum StatusSolicitacao
    {
        Pendente = 1,
        EmAnalise = 2,
        Aprovado = 3,
        Rejeitado = 4,
        Cancelado = 5
    }

    public enum TipoAparelho
    {
        Invisalign = 1,
        CorretorDireto = 2,
        SureSmile = 3,
        ClearCorrect = 4,
        SmileDirectClub = 5,
        Spark = 6
    }

    public enum PrioridadeCaso
    {
        Normal = 1,
        Urgente = 2,
        Express = 3
    }

    public enum StatusPlanejamento
    {
        Rascunho = 1,
        AguardandoAprovacao = 2,
        Aprovado = 3,
        Rejeitado = 4,
        EmProducao = 5,
        Finalizado = 6
    }

    public enum ServicoAgendamento
    {
        ConsultaInicial = 1,
        Limpeza = 2,
        Obturacao = 3,
        TratamentoCanal = 4,
        Ortodontia = 5,
        Implante = 6,
        ManutencaoAparelho = 7,
        ConsultaOrtodontica = 8,
        Outros = 9
    }

    public enum StatusAgendamento
    {
        Agendado = 1,
        Confirmado = 2,
        Realizado = 3,
        Cancelado = 4,
        Faltou = 5
    }

    public enum StatusProcesso
    {
        Iniciado = 1,
        DocumentosEnviados = 2,
        AnaliseCredito = 3,
        Planejamento = 4,
        Producao = 5,
        Entrega = 6,
        Finalizado = 7,
        Cancelado = 8
    }

    public enum StatusEtapa
    {
        Pendente = 1,
        EmAndamento = 2,
        Concluida = 3,
        Aprovada = 4,
        Rejeitada = 5,
        Aguardando = 6
    }

    public enum TipoTransacao
    {
        AprovacaoCredito = 1,
        PagamentoParcela = 2,
        Estorno = 3,
        ComissaoProfissional = 4,
        TaxaPlataforma = 5
    }

    public enum StatusTransacao
    {
        Pendente = 1,
        Processando = 2,
        Aprovado = 3,
        Rejeitado = 4,
        Estornado = 5
    }

    public enum ResultadoAnaliseCredito
    {
        AprovadoAutomatico = 1,
        AprovadoManual = 2,
        Rejeitado = 3,
        PendenteDocumentos = 4
    }

    public enum TipoAnalise
    {
        Automatica = 1,
        Manual = 2
    }

    public enum StatusInadimplencia
    {
        Ativo = 1,
        Acordo = 2,
        Pago = 3,
        Juridico = 4,
        Baixado = 5
    }

    public enum TipoDocumento
    {
        Cro = 1,
        Diploma = 2,
        Certificados = 3,
        ComprovanteEndereco = 4,
        Panoramica = 5,
        Escaneamento = 6,
        Fotos = 7,
        Outros = 8
    }

    public enum StatusDocumento
    {
        Pendente = 1,
        Processando = 2,
        Aprovado = 3,
        Rejeitado = 4
    }

    public enum EntidadeTipo
    {
        Profissional = 1,
        Paciente = 2,
        Orcamento = 3
    }
}

