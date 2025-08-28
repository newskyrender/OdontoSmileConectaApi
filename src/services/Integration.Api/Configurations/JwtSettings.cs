﻿namespace Integration.Api.Configurations
{
    // Classe para configurações específicas
    public class JwtSettings
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpirationHours { get; set; } = 8;
    }
}
