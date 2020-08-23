using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Grpc.Core;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Notebooks.Api.Exceptions;
using MicroservicesSample.Notebooks.Api.Models;
using MicroservicesSample.Notebooks.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace MicroservicesSample.Notebooks.Api.Grpc
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize]
    public sealed class NotebookGrpcService : NotebookServiceGrpc.NotebookServiceGrpcBase
    {
        private readonly INotebookService _notebookService;
        private readonly IMapper _mapper;
        private readonly IJwtHandler _jwtHandler;
        private readonly ILogger<NotebookGrpcService> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notebookService"></param>
        /// <param name="mapper"></param>
        /// <param name="jwtHandler"></param>
        /// <param name="logger"></param>
        public NotebookGrpcService(
            INotebookService notebookService,
            IMapper mapper,
            IJwtHandler jwtHandler,
            ILogger<NotebookGrpcService> logger)
        {
            _notebookService = notebookService;
            _mapper = mapper;
            _jwtHandler = jwtHandler;
            _logger = logger;
        }

        /// <inheritdoc />
        public override async Task<MessageGrpc> CreateMessage(CreateMessageGrpc request, ServerCallContext context)
        {
            var token = GetToken(context.RequestHeaders);
            try
            {
                var createdNotebook =
                    await _notebookService.CreateAsync(_mapper.Map<CreateNotebookDto>(request), token);
                var result = _mapper.Map<MessageGrpc>(createdNotebook);
                return result;
            }
            catch (Exception e)
            {
                string message = "An error occurred while creating your note";
                _logger.LogError(e, message);
                throw new RpcException(new Status(StatusCode.Aborted, message, e));
            }
        }

        /// <inheritdoc />
        public override async Task<MessageListGrpc> GetLast20(SenderId request, ServerCallContext context)
        {
            try
            {
                var resultQ =
                    await _notebookService.GetLast20ForSenderAndReceiverAsync(request.Id, CancellationToken.None);
                var result = new MessageListGrpc();
                result.Messages.AddRange(_mapper.Map<List<MessageGrpc>>(resultQ));
                return result;
            }
            catch (ApiNotebooksException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new RpcException(new Status(StatusCode.Aborted, ex.Message, ex));
            }
            catch (Exception ex)
            {
                string message = "An error occurred while getting the list of notes";
                _logger.LogError(ex, message);
                throw new RpcException(new Status(StatusCode.Aborted, message, ex));
            }
        }

        /// <inheritdoc />
        public override async Task<MessageGrpc> GetById(MessageId request, ServerCallContext context)
        {
            try
            {
                var notebook = await _notebookService.GetByIdAsync(request.Id);
                return _mapper.Map<MessageGrpc>(notebook);
            }
            catch (ApiNotebooksException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new RpcException(new Status(StatusCode.Aborted, ex.Message, ex));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while getting a note with id = {request.Id}";
                _logger.LogError(ex, message);
                throw new RpcException(new Status(StatusCode.Aborted, message, ex));
            }
        }

        private JsonWebTokenPayload GetToken(Metadata headers)
        {
            try
            {
                var bearerHeaderString = headers.Get(HeaderNames.Authorization.ToLowerInvariant());
                var tokenString = bearerHeaderString.Value.Replace("Bearer ", "");
                return _jwtHandler.GetTokenPayload(tokenString);
            }
            catch (ApiNotebooksException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new RpcException(new Status(StatusCode.Aborted, ex.Message, ex));
            }
            catch (Exception ex)
            {
                string message = $"An error occurred while reading the authorization token";
                _logger.LogError(ex, message);
                throw new RpcException(new Status(StatusCode.Unauthenticated, message, ex));
            }
        }
    }
}
