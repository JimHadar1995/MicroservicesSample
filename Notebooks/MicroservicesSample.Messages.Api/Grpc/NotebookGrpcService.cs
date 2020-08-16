using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Notebooks.Api.Models;
using MicroservicesSample.Notebooks.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;

namespace MicroservicesSample.Notebooks.Api.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public sealed class NotebookGrpcService : Notebooks.NotebookServiceGrpc.NotebookServiceGrpcBase
    {
        private readonly INotebookService _notebookService;
        private readonly IMapper _mapper;
        private readonly IJwtHandler _jwtHandler;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notebookService"></param>
        /// <param name="mapper"></param>
        /// <param name="jwtHandler"></param>
        public NotebookGrpcService(
            INotebookService notebookService,
            IMapper mapper,
            IJwtHandler jwtHandler)
        {
            _notebookService = notebookService;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
        }
        
        /// <inheritdoc />
        public override async Task<MessageGrpc> CreateMessage(CreateMessageGrpc request, ServerCallContext context)
        {
            var token = GetToken(context.RequestHeaders);
            var createdNotebook = await _notebookService.CreateAsync(_mapper.Map<CreateNotebookDto>(request), token);
            var result = _mapper.Map<MessageGrpc>(createdNotebook);
            return result;
        }

        /// <inheritdoc />
        public override async Task<MessageListGrpc> GetLast20(SenderId request, ServerCallContext context)
        {
            var resultQ = await _notebookService.GetLast20ForSenderAndReceiverAsync(request.Id, CancellationToken.None);
            var result = new MessageListGrpc();
            result.Messages.AddRange(_mapper.Map<List<MessageGrpc>>(resultQ));
            return result;
        }

        /// <inheritdoc />
        public override async Task<MessageGrpc> GetById(MessageId request, ServerCallContext context)
        {
            var notebook = await _notebookService.GetByIdAsync(request.Id);
            return _mapper.Map<MessageGrpc>(notebook);
        }

        private JsonWebTokenPayload GetToken(Metadata headers)
        {
            var bearerHeaderString = headers.Get(HeaderNames.Authorization.ToLowerInvariant());
            var tokenString = bearerHeaderString.Value.Replace("Bearer ", "");
            return _jwtHandler.GetTokenPayload(tokenString);
        }
    }
}
