namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public class UserAddDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string UserName { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public string Role { get; set; } = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public string Password { get; set; }

    }
}
