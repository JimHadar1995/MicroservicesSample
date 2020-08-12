using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Messages.Api.Entities;
using MicroservicesSample.Messages.Api.Exceptions;
using MicroservicesSample.Messages.Api.Repositories;
using MicroservicesSample.Notebooks.Api.Models;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace MicroservicesSample.Messages.Api.Services
{
    /// <inheritdoc />
    public class NotebookService : INotebookService
    {
        private readonly INotebookRepository _messageRepository;
        private readonly IRedisCacheClient _cacheClient;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageRepository"></param>
        /// <param name="cacheClient"></param>
        public NotebookService(
            INotebookRepository messageRepository,
            IRedisCacheClient cacheClient)
        {
            _messageRepository = messageRepository;
            _cacheClient = cacheClient;
        }

        /// <inheritdoc />
        public async Task<NotebookDto> CreateAsync(CreateNotebookDto model, JsonWebTokenPayload jsonWebTokenPayload, CancellationToken token = default)
        {
            var db = _cacheClient.GetDbFromConfiguration();
            if (!(await db.ExistsAsync($"user-{jsonWebTokenPayload.UserId}")))
            {
                throw new ApiNotebooksException("User not found");
            }
            var entity = new Notebook
            {
                SenderId = jsonWebTokenPayload.UserId,
                SenderName = jsonWebTokenPayload.UserName,
                Text = model.Text
            }; 
            entity = await _messageRepository.CreateAsync(entity, token);
            return GetDto(entity);
        }

        /// <inheritdoc />
        public async Task DeleteMessagesForUserAsync(string userId, CancellationToken token)
        {
            await _messageRepository.DeleteAsync(_ => _.SenderId == userId, token);
        }

        /// <inheritdoc />
        public async Task<NotebookDto> GetByIdAsync(string notebookId, CancellationToken token = default)
        {
            var entity = await _messageRepository.GetByIdAsync(notebookId, token);
            return GetDto(entity);
        }

        /// <inheritdoc />
        public async Task<List<NotebookDto>> GetLast20ForSenderAndReceiverAsync(string senderId, CancellationToken token)
        {
            var db = _cacheClient.GetDbFromConfiguration();
            if (!(await db.ExistsAsync($"user-{senderId}")))
            {
                throw new ApiNotebooksException("Sender not found");
            }
            var messages = await _messageRepository.Query
                .Where(_ => _.SenderId == senderId)
                .OrderByDescending(_ => _.Id)
                .Take(20)
                .ToListAsync(token);
            return messages.Select(m => GetDto(m)).ToList();
        }

        private NotebookDto GetDto(Notebook message)
            => new NotebookDto
            {
                CreatedAt = message.CreatedAt.ToLocalTime(),
                Id = message.Id,
                SenderId = message.SenderId,
                SenderName = message.SenderName,
                Text = message.Text
            };
    }
}
