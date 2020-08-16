using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Notebooks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MicroservicesSample.ApiGateway.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class NotebookController : BaseController
    {
        private readonly INotebooksService _notebooksService;
        /// <inheritdoc />
        public NotebookController(
            IHttpContextAccessor accessor,
            INotebooksService notebooksService)
            : base(accessor)
        {
            _notebooksService = notebooksService;
        }

        /// <summary>
        /// Получение последних 20-ти записей
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(List<MessageGrpc>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<IEnumerable<MessageGrpc>> GetLast20(CancellationToken token)
            => await _notebooksService.GetLast20Async(TokenInfo?.UserId!, token);
        
        /// <summary>
        /// Получение записи по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MessageGrpc), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<MessageGrpc> GetById(string id, CancellationToken token)
            => await _notebooksService.GetById(id, TokenInfo!, token);

        /// <summary>
        /// Создание заметки
        /// </summary>
        /// <param name="model"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(NotebookServiceGrpc), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]        
        public async Task<MessageGrpc> Create([FromBody] CreateMessageGrpc model, CancellationToken token)
            => await _notebooksService.CreateAsync(model, token);
    }
}
