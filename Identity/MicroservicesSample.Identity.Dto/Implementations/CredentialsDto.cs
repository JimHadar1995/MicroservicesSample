using System;
using System.Collections.Generic;
using System.Text;

namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// Данные для авторизации
    /// </summary>
    public class CredentialsDto
    {
        /// <summary>
        /// Имя пользователя.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        public string Password { get; set; }
    }
}
