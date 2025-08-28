using Integration.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Domain.Http.Response
{
    public class LoginResponse : ICommandResult
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string Nome { get; set; }
        public string Token { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
