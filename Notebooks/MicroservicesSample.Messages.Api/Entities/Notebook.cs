﻿using System;
using System.ComponentModel.DataAnnotations;

namespace MicroservicesSample.Notebooks.Api.Entities
{
    /// <summary>
    /// Сообщение
    /// </summary>
    public class Notebook
    {
        /// <summary>
        /// Идентификатор сообщения
        /// </summary>
        [Key]
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
