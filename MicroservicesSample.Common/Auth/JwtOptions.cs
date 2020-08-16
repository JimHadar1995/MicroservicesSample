using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// Опции для Jwt
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// Ключ
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Издатель
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Время действия
        /// </summary>
        public int ExpiryMinutes { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool ValidateAudience { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ValidAudience { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public bool ValidateIssuerSigningKey { get; set; } = false;
    }
}
