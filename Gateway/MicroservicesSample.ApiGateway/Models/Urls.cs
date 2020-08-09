namespace MicroservicesSample.ApiGateway.Models
{
    /// <summary>
    /// 
    /// </summary>
    internal class Urls
    {
        /// <summary>
        /// Urls для управления пользователями.
        /// </summary>
        public string Identity { get; set; } = string.Empty;

        /// <summary>
        /// Urls для записей пользователя.
        /// </summary>
        public string Notebooks { get; set; } = string.Empty;
    }
}
