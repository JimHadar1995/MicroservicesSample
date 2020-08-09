using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using MicroservicesSample.Common.Auth;
using MicroservicesSample.Common.Database;
using MicroservicesSample.Common.Exceptions;
using MicroservicesSample.Identity.Core.Entities;
using MicroservicesSample.Identity.Core.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace MicroservicesSample.Identity.Application.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class RefreshTokenService : IRefreshTokenService
    {
        private static readonly string[] SpecialChars = new[] { "/", "\\", "=", "+", "?", ":", "&" };
        private readonly IUnitOfWork _ufw;
        private readonly IJwtHandler _jwtHandler;
        private readonly UserManager<User> _userManager;
        private IRepository<RefreshToken> _refreshTokenRepo => _ufw.Repository<RefreshToken>();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ufw"></param>
        /// <param name="jwtHandler"></param>
        /// <param name="userManager"></param>
        public RefreshTokenService(
            IUnitOfWork ufw,
            IJwtHandler jwtHandler,
            UserManager<User> userManager)
        {
            _ufw = ufw;
            _userManager = userManager;
            _jwtHandler = jwtHandler;
        }

        /// <inheritdoc />
        public async Task<string> AddAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId).ConfigureAwait(false);
            if (user == null)
                throw new EntityNotFoundException();

            var token = await _refreshTokenRepo.CreateAsync(new RefreshToken
            {
                UserId = user.Id,
                User = user,
                Token = Generate()
            });
            return token.Token;
        }

        /// <inheritdoc />
        public async Task<JsonWebToken> CreateAccessTokenAsync(string refreshToken)
        {
            var refreshTokenE = await _refreshTokenRepo.FirstOrDefaultAsync(_ => _.Token == refreshToken);
            if (refreshTokenE == null)
            {
                throw new RefreshTokenNotFoundException();
            }
            var user = refreshTokenE.User;
            var jwt = _jwtHandler.CreateToken(user.Id, user.UserName, user.Roles.First().Role.Name);
            jwt.RefreshToken = await AddAsync(user.Id).ConfigureAwait(false);
            await _refreshTokenRepo.DeleteAsync(refreshTokenE);
            return jwt;
        }

        /// <inheritdoc />
        public string Generate(int length = 50, bool removeSpecialChars = true)
        {
            using var rng = new RNGCryptoServiceProvider();
            var bytes = new byte[length];
            rng.GetBytes(bytes);
            var result = Convert.ToBase64String(bytes);

            return removeSpecialChars
                ? SpecialChars.Aggregate(result, (current, chars) => current.Replace(chars, string.Empty))
                : result;
        }
    }
}
