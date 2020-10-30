using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using TokenUtility.Configuration;
using System.Text;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace TokenUtility.Tokenservice
{
    public static class JWTServiceExtension
    {
        public static void AddJWTService(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSection = configuration.GetSection(nameof(JwtSecrets));
            services.Configure<JwtSecrets>(jwtSection);
            services.AddSingleton<IJwtSecrets>(secrets => secrets.GetRequiredService<IOptions<JwtSecrets>>().Value);
            var jwtSecrets = jwtSection.Get<JwtSecrets>();
            var key = Encoding.ASCII.GetBytes(jwtSecrets.userTokenKey);

            services.AddSingleton<IJWTTokenGenerator, JWTTokenGenerator>();

            services.AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(auth =>
            {
                auth.RequireHttpsMetadata = false;
                auth.SaveToken = false;
                auth.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    RequireSignedTokens = true,
                    RequireExpirationTime = true,
                    ValidateLifetime = true,
                };
            });
        }
    }
}
