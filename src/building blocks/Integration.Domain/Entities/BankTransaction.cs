using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class BankTransaction: Entity
    {
        protected BankTransaction() { }

        public BankTransaction(Guid id, double amount, string transactionId, DateTime requestDate, DateTime paymentDate, 
            string description, string authenticationCode, bool paymentConfirmed, Guid bankId, Guid companyId, 
            Guid userId, StatusCodeType statusCodeType, string qrCode, Guid bankAccountId)
        {
            if (id != Guid.Empty) Id = id;
            Amount = amount;
            TransactionId = transactionId;
            RequestDate = requestDate;
            PaymentDate = paymentDate;
            Description = description;
            AuthenticationCode = authenticationCode;
            PaymentConfirmed = paymentConfirmed;
            BankId = bankId;
            CompanyId = companyId;
            BankAccountId = bankAccountId;
            UserId = userId;
            StatusCodeType = statusCodeType;
            QrCode = qrCode;

            // Validation temporarily disabled
            // new ValidationContract<BankTransaction>(this)
            //     ;

        }

        public double Amount { get; private set; }
        public string TransactionId { get; private set; }
        public DateTime RequestDate { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string Description { get; private set; }
        public string AuthenticationCode { get; private set; }
        public string QrCode { get; private set; }
        public bool PaymentConfirmed { get; private set; }
        public StatusCodeType StatusCodeType { get; private set; }

        public Guid BankId { get; private set; }
        public Bank Bank { get; set; }

        public Guid BankAccountId { get; set; }
        public BankAccount BankAccount { get; set; }

        public Guid CompanyId { get; private set; }
        public Company Company { get; set; }

        public Guid UserId { get; private set; }
        public User User { get; set; }
    }
}

