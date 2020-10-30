using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using DataManagers.AuthManagers.interfaces;
using TokenUtility.Tokenservice;
using System.Security.Claims;
using HelperServices;
using TokenUtility.Configuration;
using Microsoft.AspNetCore.Authorization;
using Models.Entities;
using DataManagers.Managers.interfaces;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HotelManagement.Controllers
{
    [Route("v1/identity")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IAdminManager _adminManager;
        private IJWTTokenGenerator _jWTTokenGenerator;
        private IJwtSecrets _jwtSecrets;
        private ISessionDataManager _sessionDataManager;
        public IdentityController(IAdminManager adminManager, IJWTTokenGenerator jWTTokenGenerator,IJwtSecrets jwtSecrets,ISessionDataManager sessionDataManager)
        {
            _adminManager = adminManager;
            _jWTTokenGenerator = jWTTokenGenerator;
            _jwtSecrets = jwtSecrets;
            _sessionDataManager = sessionDataManager;
        }
        // POST v1/identity
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] SignInDto signInDto)
        {
            var user =await _adminManager.SigninAsync(signInDto.UserName, signInDto.Password);

            if (user == null)
            {
                return BadRequest("Username or password incorrect");
            }

            var claims = new Claim[]
            {
                new Claim(ZeroClaims.UserId,user.UserId.ToString())
            };

            var browserId = HttpContext.Request.Headers["bfg"];
            var preSession = await _sessionDataManager.GetSessionByIdAsync(browserId);

            string refreshToken = null;

            if (preSession == null)
            {
                refreshToken = await _jWTTokenGenerator.GenerateRefreshToken();
                var session = new Session
                {
                    Id = browserId,
                    UserId = user.UserId,
                    RefreshToken = refreshToken,
                    RefreshExpiary = DateTime.UtcNow.AddDays(15)
                };
                var response = await _sessionDataManager.CreateSessionAsync(session);
            }
            else
            {
                refreshToken = preSession.RefreshToken;
            }

            string accessToken = await _jWTTokenGenerator.GenerateAccessToken(claims, _jwtSecrets.userTokenKey, DateTime.UtcNow.AddHours(3));

            var token = new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };

            return Ok(token);
        }

        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetAdmin()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = identity.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);

            var user = await _adminManager.GetUserByIdAsync(id);

            return Ok(user);
        }

        [HttpDelete("signout")]
        [Authorize]
        public async Task<IActionResult> SignOut()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = identity.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);
            var browserId = HttpContext.Request.Headers["bfg"];
            var session = await _sessionDataManager.GetSessionForUserandId(id, browserId);

            if(session != null)
            {
                await _sessionDataManager.DeleteSessionByAsync(session);
            }

            var status = new
            {
                Succeed = true
            };

            return Ok(status);
        }

        [HttpPost("bexiotoken")]
        [Authorize]
        public async Task<IActionResult> ChangeBexioToken(Admin admin)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = identity.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);

            var update = await _adminManager.ChangeBexioTokenAsync(id, admin);

            return Ok(update);
        }

        // PUT v1/identity
        [HttpPut("reset")]
        [Authorize]
        public async Task<IActionResult> PasswordReset([FromBody] PasswordResetDto passwordResetDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = identity.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);
            await _adminManager.ResetPassword(id, passwordResetDto.Password);

            return Ok();
        }

        // PUT v1/identity/refresh
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenModel tokenModel)
        {
            Utility.CheckNotNull(tokenModel, nameof(tokenModel));

            string accessToken = tokenModel.AccessToken;
            string refreshToken = tokenModel.RefreshToken;

            var browserId = HttpContext.Request.Headers["bfg"];
            var claims = _jWTTokenGenerator.GetClaimsFromExpiredToken(accessToken);
            string userId = claims.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);
            Session session = null;
            if (browserId.Count!=0)
            {
                session = await _sessionDataManager.GetSessionForUserandId(id, browserId);
            }

            if (session == null || session.RefreshToken != refreshToken || session.RefreshExpiary <= DateTime.UtcNow)
            {
                return Unauthorized("REFRESH_EXPIRE");
            }

            var newClaim = new Claim[]
            {
                new Claim(ZeroClaims.UserId,userId)
            };

            var newAccessToken = await _jWTTokenGenerator.GenerateAccessToken(newClaim, _jwtSecrets.userTokenKey, DateTime.UtcNow.AddHours(3));
            var newRefreshToken = await _jWTTokenGenerator.GenerateRefreshToken();

            session.RefreshToken = newRefreshToken;
            session.RefreshExpiary = DateTime.UtcNow.AddDays(10);

            await _sessionDataManager.UpdateSessionAsync(session);

            tokenModel.AccessToken = newAccessToken;
            tokenModel.RefreshToken = newRefreshToken;

            return Ok(tokenModel);
        }

        [HttpPost("resetpassword")]
        [Authorize]
        public async Task<IActionResult> UpdatePassword([FromBody] SignInDto signInDto)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            string userId = identity.FindFirst(ZeroClaims.UserId)?.Value;

            int id = Int32.Parse(userId);

            var admin = await _adminManager.GetUserByIdAsync(id);
            admin.Password = signInDto.Password;
            admin.UserName = signInDto.UserName;

            await _adminManager.UpdateUser(admin);

            return Ok(signInDto);
        }
    }
}
