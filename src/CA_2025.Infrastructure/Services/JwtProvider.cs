using CA_2025.Application.Services;
using CA_2025.Domain.Users;
using CA_2025.Infrastructure.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CA_2025.Infrastructure.Services
{
    public sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        public Task<string> CreateTokenAsync(AppUser user, CancellationToken cancellationToken = default)
        {
            var expires = DateTime.Now.AddDays(1);

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(options.Value.SecretKey));

            SigningCredentials signingCredentials = new(
                key: key,
                algorithm: SecurityAlgorithms.HmacSha512);

            List<Claim> claims =
            [
                new Claim("user-id", user.Id.ToString())
            ];

            JwtSecurityToken securityToken = new(
                issuer: options.Value.Issuer,
                audience: options.Value.Audience,
                claims: claims.AsEnumerable(),
                notBefore: DateTime.Now,
                expires: expires,
                signingCredentials: signingCredentials);

            JwtSecurityTokenHandler handler = new();

            string token = handler.WriteToken(securityToken);

            return Task.FromResult(token);
        }
    }
}
