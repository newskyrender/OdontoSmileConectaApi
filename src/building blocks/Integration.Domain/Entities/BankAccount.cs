using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class BankAccount: Entity
    {
        protected BankAccount() { }

        public BankAccount(Guid id, string accountNumber, string accountDigit, 
            AccountType accountType, Guid bankId, string agency, bool active)
        {
            if (id != Guid.Empty) Id = id;

            BankId = bankId;
            Agency = agency;
            AccountNumber = accountNumber;
            AccountDigit = accountDigit;
            AccountType = accountType;
            Active = active;

            // Validation temporarily disabled
            // new ValidationContract<BankAccount>(this)
            //     .IsRequired(x => x.AccountNumber, "O número da conta bancária deve ser informada informado")
            //     ;
        }

        public string AccountNumber { get; private set; }
        public string AccountDigit { get; set; }
        public string Agency { get; set; }
        public AccountType AccountType { get; set; }
        public bool Active { get; private set; }

        public Guid BankId { get; set; }
        public Bank Bank { get; set; }

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;

    }
}

