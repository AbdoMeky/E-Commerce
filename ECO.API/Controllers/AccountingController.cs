using ECO.CORE.DTO.AccountingDTO;
using ECO.CORE.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;

namespace ECO.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountingController : ControllerBase
    {
        private readonly IAccountingRepository _accountingRepository;
        public AccountingController(IAccountingRepository accountingRepository)
        {
            this._accountingRepository = accountingRepository;
        }
        [HttpPost("RegisteUser")]
        public async Task<ActionResult> RegisteUser(RegisteUserDTO user)
        {
            if (ModelState.IsValid)
            {
                var result = await _accountingRepository.RegisteUser(user);
                if (result.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(result.RefreshToken))
                    {
                        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                    }
                    return Ok(result);
                }
                return BadRequest(result.Massage);
            }
            return BadRequest(ModelState);
        }
        [HttpPost("RegisteSeller")]
        public async Task<ActionResult> RegisteSeller(RegisteSellerDTO seller)
        {
            var result = await _accountingRepository.RegisteSeller(seller);
            if (result.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(result.RefreshToken))
                {
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                }
                return Ok(result);
            }
            return BadRequest(result.Massage);
        }
        [HttpPost("LgIn")]
        public async Task<ActionResult> LogIn(LogInDTO logInDTO)
        {
            if (ModelState.IsValid)
            {
                var result =await _accountingRepository.LogIn(logInDTO);
                if (result.IsAuthenticated)
                {
                    if (!string.IsNullOrEmpty(result.RefreshToken))
                    {
                        SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                    }
                    return Ok(result);
                }
                return BadRequest(result.Massage);
            }
            return BadRequest(ModelState);
        }
        [HttpGet("RevokeAndInvoke")]
        public async Task<ActionResult> RevokeAndInvoke()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _accountingRepository.CheckRefreshTokenAndRevoke(refreshToken);
            if (result.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(result.RefreshToken))
                {
                    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpired);
                }
                return Ok(result);
            }
            return BadRequest(result.Massage);
        }
        [HttpGet("Revoke")]
        public async Task<ActionResult> Revoke()
        {
            var refreshToken = Request.Cookies["refreshToken"];
            var result = await _accountingRepository.RevokeRefreshToken(refreshToken);
            if (result)
            {
                return Ok();
            }
            return BadRequest("no valid refresh token to revoke");
        }
        void SetRefreshTokenInCookie(string refreshToken,DateTime? expiresIn) 
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expiresIn?.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };
            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
