using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Entities
{
    public class SolicitacaoOrcamento : Entity
    {
        protected SolicitacaoOrcamento() { }

        public SolicitacaoOrcamento(Guid id, string numeroPedido, string nomeCompleto, string cpf,
            string telefone, TipoTratamento tipoTratamento)
        {
            if (id != Guid.Empty) Id = id;
            NumeroPedido = numeroPedido;
            NomeCompleto = nomeCompleto;
            Cpf = cpf;
            Telefone = telefone;
            TipoTratamento = tipoTratamento;
            Status = StatusSolicitacao.Pendente;

            new ValidationContract<SolicitacaoOrcamento>(this)
                .IsRequired(x => x.NumeroPedido, "O número do pedido deve ser informado")
                .IsRequired(x => x.NomeCompleto, "O nome completo deve ser informado")
                .IsRequired(x => x.Cpf, "O CPF deve ser informado")
                .IsRequired(x => x.Telefone, "O telefone deve ser informado");
        }

        public string NumeroPedido { get; private set; }
        public Guid? PacienteId { get; private set; }
        public string NomeCompleto { get; private set; }
        public string Cpf { get; private set; }
        public string Telefone { get; private set; }
        public string Email { get; private set; }
        public TipoTratamento TipoTratamento { get; private set; }
        public string Observacoes { get; private set; }
        public StatusSolicitacao Status { get; private set; }
        public decimal? ValorAprovado { get; private set; }
        public int? NumeroParcelas { get; private set; }
        public decimal? ValorParcela { get; private set; }
        public Guid? ProfissionalId { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        public virtual Paciente Paciente { get; private set; }
        public virtual Profissional Profissional { get; private set; }

        public void AtualizarStatus(StatusSolicitacao novoStatus)
        {
            Status = novoStatus;
            UpdatedAt = DateTime.UtcNow;
        }

        public void AprovarOrcamento(decimal valorAprovado, int numeroParcelas, decimal valorParcela)
        {
            Status = StatusSolicitacao.Aprovado;
            ValorAprovado = valorAprovado;
            NumeroParcelas = numeroParcelas;
            ValorParcela = valorParcela;
            UpdatedAt = DateTime.UtcNow;
        }
    }

}
