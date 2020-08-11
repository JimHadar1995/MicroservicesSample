namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PasswordPolicy 
    {
        /// <summary>
        /// 
        /// </summary>
        public bool RequireDigit { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int RequiredLength { get; set; }
    }
}
