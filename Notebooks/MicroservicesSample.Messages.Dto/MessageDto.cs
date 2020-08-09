using System;

namespace MicroservicesSample.Notebooks.Dto
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NotebookDto
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Текст сообщения
        /// </summary>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Идентификатор пользователя, отправившего сообщение.
        /// </summary>
        public string SenderId { get; set; } = string.Empty;

        /// <summary>
        /// Имя пользователя, отправившего сообщение.
        /// </summary>
        public string SenderName { get; set; } = string.Empty;

        /// <summary>
        /// Время и дата создания сообщения.
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
