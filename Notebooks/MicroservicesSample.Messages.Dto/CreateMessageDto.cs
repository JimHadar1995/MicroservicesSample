﻿namespace MicroservicesSample.Notebooks.Dto
{
    /// <summary>
    /// Модель создания сообщения
    /// </summary>
    public sealed class CreateNotebookDto
    {
        /// <summary>
        /// Текст сообщения.
        /// </summary>
        public string Text { get; set; } = string.Empty;

    }
}
