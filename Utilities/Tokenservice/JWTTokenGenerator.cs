using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TokenUtility.Configuration;

namespace TokenUtility.Tokenservice
{
    internal class JWTTokenGenerator : IJWTTokenGenerator
    {
        private JwtSecurityTokenHandler tokenHandler;
        private SecurityTokenDescriptor tokenDescriptor;
        private readonly IJwtSecrets _jwtSecrets;
        public JWTTokenGenerator(IJwtSecrets jwtSecrets)
        {
            tokenHandler = new JwtSecurityTokenHandler();
            _jwtSecrets = jwtSecrets;
        }

        public async Task<string> GenerateAccessToken(Claim[] claims, string secretKey, DateTime dateTime)
        {
            var key = Encoding.ASCII.GetBytes(secretKey);

            var result = await Task.Run(() =>
            {
                tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Expires = dateTime,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenHandler.CreateToken(tokenDescriptor);
                string tokenResult = tokenHandler.WriteToken(token);
                return tokenResult;
            });
            
            return result;
        }

        public async Task<string> GenerateRefreshToken()
        {
            var result = await Task.Run(() =>
            {
                var randomNumber = new byte[32];
                using (var rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(randomNumber);
                    return Convert.ToBase64String(randomNumber);
                }
            });

            return result;
        }

        public ClaimsPrincipal GetClaimsFromExpiredToken(string token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtSecrets.userTokenKey)),
                ValidateLifetime = false
            };

            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if(jwtSecurityToken==null || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return claimsPrincipal;
        }

        public ClaimsPrincipal ValidateJWT(string token, string onetimeKey)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey =  new SymmetricSecurityKey(Encoding.ASCII.GetBytes(onetimeKey)),
                ValidateIssuer = false,
                ValidateAudience = false,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };

            SecurityToken securityToken;
            var claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);

            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256Signature, StringComparison.InvariantCultureIgnoreCase))
            {
                return null;
            }

            return claimsPrincipal;
        }
    }

    public interface IJWTTokenGenerator
    {
        public Task<string> GenerateAccessToken(Claim[] claims, string secretKey,DateTime dateTime);
        public Task<string> GenerateRefreshToken();
        public ClaimsPrincipal GetClaimsFromExpiredToken(string token);
        public ClaimsPrincipal ValidateJWT(string token, string oneTimeKey);
    }
}
