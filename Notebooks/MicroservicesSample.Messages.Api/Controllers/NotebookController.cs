using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Messages.Api.Services;
using MicroservicesSample.Notebooks.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.Notebooks.Api.Controllers
{
    /// <summary>
    /// Контроллер для работы с записями
    /// </summary>
    [Authorize]
    public class NotebookController : BaseController
    {
        private readonly INotebookService _messageService;

        /// <inheritdoc />
        public NotebookController(IHttpContextAccessor accessor, INotebookService messageService) 
            : base(accessor)
        {
            _messageService = messageService;
        }

        /// <summary>
        /// Создание записи
        /// </summary>
        /// <param name="message"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(NotebookDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<NotebookDto> Create([FromBody] CreateNotebookDto message, CancellationToken token)
            => await _messageService.CreateAsync(message, TokenInfo!, token);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotebookDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<NotebookDto> GetById(string id, CancellationToken token)
            => await _messageService.GetByIdAsync(id, token);

        /// <summary>
        /// Возвращает последние 20 записей для указанного отправителя.
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<NotebookDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<List<NotebookDto>> GetLast20(string senderId, CancellationToken token)
            => await _messageService.GetLast20ForSenderAndReceiverAsync(senderId, token);
    }
}
