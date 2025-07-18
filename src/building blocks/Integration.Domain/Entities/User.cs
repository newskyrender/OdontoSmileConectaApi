using FluentValidator;
using Integration.Domain.Common;
using Integration.Domain.Enums;

namespace Integration.Domain.Entities
{
    public class User: Entity
    {
        protected User() { }

        public User(Guid id, string name, string email, string phoneNumber, UserType userType, bool active, string password)
        {
            if (id != Guid.Empty) Id = id;

            Name = name;
            Password = password; // Encryption temporarily disabled
            Email = email;
            PhoneNumber = phoneNumber;
            UserType = userType;
            Active = active;

            // Validation temporarily disabled
            // new ValidationContract<User>(this)
            //     .IsRequired(x => x.Name, "O nome deve ser informado")
            //     .IsRequired(x => x.Email, "O e-mail deve ser informado")
            //     .IsEmail(x => x.Email, "O e-mail informado deve ser um e-mail válido")
            //     ;

        }

        public string Name { get; private set; }
        public string Password { get; private set; }
        public string Email { get; private set; }
        public string PhoneNumber { get; private set; }
        public UserType UserType { get; private set; }
        public bool Active { get; private set; }

        public void Activate() => Active = true;
        public void Deactivate() => Active = false;
    }
}

