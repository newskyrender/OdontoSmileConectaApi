using FluentValidator;
using Integration.Domain.Common;

namespace Integration.Domain.Entities
{
    public class Company: Entity
    {
        protected Company() { }

        public Company(Guid id, string companyName, string tradingName, string cnpj, string email, string phoneNumber, string contact, bool active)
        {
            if (id != Guid.Empty) Id = id;
            CompanyName = companyName;
            TradingName = tradingName;
            Cnpj = cnpj;
            Email = email;
            PhoneNumber = phoneNumber;
            Contact = contact;
            Active = active;

            // Validation temporarily disabled
            // new ValidationContract<Company>(this)
            //     .IsRequired(x => x.CompanyName, "A razão social deve ser informada informada")
            //     .IsRequired(x => x.TradingName, "O nome fantasia deve ser informado")
            //     .IsRequired(x => x.Cnpj, "O CNPJ deve ser informado")
            //     .IsRequired(x => x.PhoneNumber, "O número do telefone deve ser informado")
            //     .IsRequired(x => x.Email, "O e-mail deve ser informado")
            //     .IsRequired(x => x.Contact, "O nome do contato deve ser informado")
            //     .IsEmail(x => x.Email, "O e-mail informado deve ser um e-mail válido")
            //     ;
        }

        public string CompanyName { get; private set; }
        public string TradingName { get; private set; }
        public string Cnpj { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public string Contact { get; private set; }
        public bool Active { get; private set; }

        public IEnumerable<BankTransaction> BankTransactions { get; } = new List<BankTransaction>();

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;

    }
}

