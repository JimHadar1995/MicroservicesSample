using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// Атрибут для JWT авторизации
    /// </summary>
    public sealed class JwtAuthAttribute : AuthAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="policy"></param>
        public JwtAuthAttribute(string policy = "") : base(JwtBearerDefaults.AuthenticationScheme, policy)
        {
        }
    }
}
