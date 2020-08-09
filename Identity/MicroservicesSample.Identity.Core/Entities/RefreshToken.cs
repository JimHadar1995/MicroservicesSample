using System;
using System.ComponentModel.DataAnnotations;
using MicroservicesSample.Identity.Core.Exceptions;

namespace MicroservicesSample.Identity.Core.Entities
{
    /// <summary>
    /// Refresh token entity
    /// </summary>
    public class RefreshToken
    {
        /// <summary>
        /// User id
        /// </summary>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// refresh token
        /// </summary>
        [Key]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// Created date
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// User
        /// </summary>
        public virtual User User { get; set; } = null!;

        /// <summary>
        /// 
        /// </summary>
        public RefreshToken()
        {

        }
    }
}
