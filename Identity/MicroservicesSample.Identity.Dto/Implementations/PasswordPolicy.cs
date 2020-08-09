namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class PasswordPolicy
    {
        /// <summary>
        /// Необходимы ли цифры в пароле.
        /// </summary>
        public bool RequireDigit { get; set; }

        /// <summary>
        /// Необходимы ли символы в нижнем регистре.
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// Необходимы ли символы в нижнем регистре.
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// Минимальная длина пароля.
        /// </summary>
        public int RequiredLength { get; set; }
    }
}
