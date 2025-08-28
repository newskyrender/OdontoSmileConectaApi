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
    public class Documento : Entity
    {
        protected Documento() { }

        public Documento(Guid id, EntidadeTipo entidadeTipo, Guid entidadeId, TipoDocumento tipoDocumento,
            string nomeOriginal, string nomeArquivo, string caminhoArquivo, int tamanhoBytes, string tipoMime)
        {
            if (id != Guid.Empty) Id = id;
            EntidadeTipo = entidadeTipo;
            EntidadeId = entidadeId;
            TipoDocumento = tipoDocumento;
            NomeOriginal = nomeOriginal;
            NomeArquivo = nomeArquivo;
            CaminhoArquivo = caminhoArquivo;
            TamanhoBytes = tamanhoBytes;
            TipoMime = tipoMime;
            Status = StatusDocumento.Pendente;

            new ValidationContract<Documento>(this)
                .IsRequired(x => x.NomeOriginal, "O nome original do arquivo deve ser informado")
                .IsRequired(x => x.NomeArquivo, "O nome do arquivo deve ser informado")
                .IsRequired(x => x.CaminhoArquivo, "O caminho do arquivo deve ser informado")
                .IsGreaterThan(x => x.TamanhoBytes, 0, "O tamanho do arquivo deve ser maior que zero");
        }

        public EntidadeTipo EntidadeTipo { get; private set; }
        public Guid EntidadeId { get; private set; }
        public TipoDocumento TipoDocumento { get; private set; }
        public string NomeOriginal { get; private set; }
        public string NomeArquivo { get; private set; }
        public string CaminhoArquivo { get; private set; }
        public int TamanhoBytes { get; private set; }
        public string TipoMime { get; private set; }
        public StatusDocumento Status { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

        public void AtualizarStatus(StatusDocumento novoStatus)
        {
            Status = novoStatus;
        }
    }
}
