using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class Charge: Entity
    {
        protected Charge() { }

        public Charge(Guid id, string accountNumber, string accountDigit,
            AccountType accountType, Guid bankId, decimal valor)        
        {
            if (id != Guid.Empty) Id = id;

            BankId = bankId;
            AccountNumber = accountNumber;
            AccountDigit = accountDigit;
            AccountType = accountType;
            Valor = valor;

            // Validation temporarily disabled
            // new ValidationContract<Charge>(this)
            //     .IsRequired(x => x.AccountNumber, "O número da conta bancária deve ser informada informado")
            //     ;
        }
        public string AccountNumber { get; private set; }
        public string AccountDigit { get; set; }
        public AccountType AccountType { get; set; }       
        public decimal Valor { get; set; }
        public Guid BankId { get; set; }
        public Bank Bank { get; set; }

        public string EndToEnd { get; set; }
        public string QrCode { get; set; }
    }
}

