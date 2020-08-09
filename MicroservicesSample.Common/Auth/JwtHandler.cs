using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using MicroservicesSample.Common.Exceptions;
using Microsoft.IdentityModel.Tokens;

namespace MicroservicesSample.Common.Auth
{
    /// <inheritdoc />
    public class JwtHandler : IJwtHandler
    {
        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtRegisteredClaimNames.Sub,
            JwtRegisteredClaimNames.UniqueName,
            JwtRegisteredClaimNames.Jti,
            JwtRegisteredClaimNames.Iat,
            ClaimTypes.Role,
            ClaimTypes.NameIdentifier
        };

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public JwtHandler(JwtOptions options)
        {
            _options = options;
            var issuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(issuerSigningKey, SecurityAlgorithms.HmacSha256);
            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issuerSigningKey,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.ValidAudience,
                ValidateAudience = _options.ValidateAudience,
                ValidateLifetime = true
            };
        }

        /// <inheritdoc />
        public JsonWebToken CreateToken(string userId, string userName, string role, IDictionary<string, string>? claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User id claim can not be empty.", nameof(userId));
            }

            var now = DateTime.UtcNow;
            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, now.ToTimestamp().ToString()),
            };
            if (!string.IsNullOrWhiteSpace(role))
            {
                jwtClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            if (!string.IsNullOrWhiteSpace(userName))
            {
                jwtClaims.Add(new Claim(ClaimTypes.NameIdentifier, userName));
            }

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray()
                               ?? Array.Empty<Claim>();
            jwtClaims.AddRange(customClaims);
            var expires = now.AddMinutes(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: jwtClaims,
                notBefore: now,
                expires: expires,
                signingCredentials: _signingCredentials,
                audience: _options.ValidAudience
            );
            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return new JsonWebToken
            {
                AccessToken = token,
                RefreshToken = string.Empty,
                Expires = expires.ToTimestamp(),
                Id = userId,
                Role = role ?? string.Empty,
                Claims = customClaims.ToDictionary(c => c.Type, c => c.Value)
            };
        }

        /// <inheritdoc />
        public JsonWebTokenPayload GetTokenPayload(string accessToken)
        {
            _jwtSecurityTokenHandler.ValidateToken(accessToken, _tokenValidationParameters,
                out var validatedSecurityToken);
            if (!(validatedSecurityToken is JwtSecurityToken jwt))
            {
                throw new SecurityException("Invalid access token");
            }

            return new JsonWebTokenPayload
            {
                UserId = jwt.Subject,
                UserName = jwt.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value,
                Role = jwt.Claims.Single(x => x.Type == ClaimTypes.Role).Value,
                Expires = jwt.ValidTo.ToTimestamp(),
                Claims = jwt.Claims.Where(x => !DefaultClaims.Contains(x.Type))
                    .ToDictionary(k => k.Type, v => v.Value)
            };
        }
    }
}
