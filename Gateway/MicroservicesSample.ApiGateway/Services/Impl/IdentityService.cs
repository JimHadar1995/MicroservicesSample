using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using MicroservicesSample.ApiGateway.Services.Contrants;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Consul;
using MicroservicesSample.Identity.Dto;
using MicroservicesSample.Identity.Dto.Implementations;

namespace MicroservicesSample.ApiGateway.Services.Impl
{
    /// <inheritdoc />
    public class IdentityService : BaseApiService, IIdentityService
    {
        /// <inheritdoc />
        public IdentityService(HttpClient httpClient, IConsulServicesRegistry servicesRegistry) : base(httpClient,
            servicesRegistry)
        {
        }

        /// <inheritdoc />
        public override string ServiceName => "Identity";

        /// <inheritdoc />
        public Task<JsonWebToken> LoginAsync(CredentialsDto credentials, CancellationToken token)
            => PostAsync<JsonWebToken>("api/auth/login", credentials, token);

        /// <inheritdoc />
        public Task<List<RoleDto>> GetAllRolesAsync(CancellationToken token)
            => GetAsync<List<RoleDto>>("api/role", token);

        /// <inheritdoc />
        public Task<List<UserDto>> GetAllUsersAsync(CancellationToken token)
            => GetAsync<List<UserDto>>("api/user/", token);

        /// <inheritdoc />
        public Task<UserDto> GetUserByIdAsync(string userId, CancellationToken token)
            => GetAsync<UserDto>($"api/user/{userId}", token);

        /// <inheritdoc />
        public Task<UserDto> CreateUserAsync(UserAddDto model, CancellationToken token)
            => PostAsync<UserDto>("api/user", model, token);

        /// <inheritdoc />
        public Task<UserDto> UpdateUserAsync(UserUpdateDto model, CancellationToken token)
            => PutAsync<UserDto>("api/user", model, token);

        /// <inheritdoc />
        public Task DeleteUserAsync(string id, CancellationToken token)
            => DeleteAsync($"api/user/{id}", token);
    }
}
