using FluentValidator;
using Integration.Domain.Common;

namespace Integration.Domain.Entities
{
    public class Usuario : Entity
    {
        protected Usuario() { }

        public Usuario(Guid id, string email, string senhaHash, string nome, bool ativo = true)
        {
            if (id != Guid.Empty) Id = id;
            Email = email;
            SenhaHash = senhaHash;
            Nome = nome;
            Ativo = ativo;

            Validar();
        }

        public string Email { get; private set; }
        public string SenhaHash { get; private set; }
        public string Nome { get; private set; }
        public bool Ativo { get; private set; }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

        private void Validar()
        {
            new ValidationContract<Usuario>(this)
                .IsRequired(x => x.Email, "O e-mail deve ser informado")
                .IsEmail(x => x.Email, "O e-mail informado deve ser válido")
                .IsRequired(x => x.Nome, "O nome deve ser informado")
                .IsRequired(x => x.SenhaHash, "A senha deve ser informada");
        }

        public void AtualizarSenha(string novaSenhaHash)
        {
            SenhaHash = novaSenhaHash;
            UpdatedAt = DateTime.UtcNow;
        }

        public void Ativar() => Ativo = true;
        public void Desativar() => Ativo = false;
    }
}