using Microsoft.AspNetCore.Authorization;

namespace MicroservicesSample.Common.Auth
{
    /// <summary>
    /// 
    /// </summary>
    public class AuthAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="scheme"></param>
        /// <param name="policy"></param>
        public AuthAttribute(string scheme, string policy = "") : base(policy)
        {
            AuthenticationSchemes = scheme;
        }
    }
}
