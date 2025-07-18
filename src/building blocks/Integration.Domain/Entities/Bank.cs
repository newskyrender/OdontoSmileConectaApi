using FluentValidator;
using Integration.Domain.Common;

namespace Integration.Domain.Entities
{
    public class Bank: Entity
    {
        protected Bank() { }

        public Bank(Guid id, string name, int bankNumber, bool active)
        {
            if (id != Guid.Empty) Id = id;
            Name = name;
            BankNumber = bankNumber;
            Active = active;

            // Validation temporarily disabled
            // new ValidationContract<Bank>(this)
            //     .IsRequired(x => x.Name, "O nome do banco deve ser informado")
            //     ;
        }

        public string Name { get; private set; }
        public int BankNumber { get; private set; }
        public bool Active { get; private set; }

        public IEnumerable<BankAccount> BankAccounts { get; } = new List<BankAccount>();

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;

    }
}

