using FluentValidator;
using Integration.Domain.Common;

namespace Integration.Domain.Entities
{
    public class Fake: Entity
    {
        protected Fake() { }

        public Fake(Guid id, string nome, string email)
        {
            if (id != Guid.Empty) Id = id;
            Nome = nome;
            Email = email;

            // Validation temporarily disabled
            // new ValidationContract<Fake>(this)
            //     .IsRequired(x => x.Nome, "O nome deve ser informado")
            //     .IsRequired(x => x.Email, "O e-mail deve ser informado")
            //     .IsEmail(x => x.Email, "O e-mail informado deve ser um e-mail válido")
            //     ;
        }

        public string Nome { get; private set; }
        public string Email { get; private set; }

    }
}

