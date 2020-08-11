namespace MicroservicesSample.Identity.Dto
{
    /// <summary>
    /// Модель обновления пользователя
    /// </summary>
    public class UserUpdateDto : UserAddDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; } = string.Empty;
    }
}
