using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECO.CORE.DTO.AccountingDTO;
using ECO.CORE.Entities;
using ECO.CORE.Interface;
using ECO.EF.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ECO.EF.Repository
{
    public class AccountingRepository : IAccountingRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public AccountingRepository(AppDbContext context,UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._context=context;
        }

        public async Task<AccoutResultDTO> LogIn(LogInDTO logInDTO)
        {
            ApplicationUser user = await _userManager.FindByEmailAsync(logInDTO.Email);
            if (user is null || !await _userManager.CheckPasswordAsync(user, logInDTO.Password))
            {
                return new AccoutResultDTO { Massage = "Email or Password is not correct" };
            }
            JwtSecurityToken JwtToken = await CreateToken(user);
            var refreshToken = new RefreshToken();
            if(user.RefreshTokens.Any(t=>t.IsActive))
            {
                refreshToken=user.RefreshTokens.FirstOrDefault(t=>t.IsActive);
            }
            else
            {
                refreshToken = GenerateRefreshToken();
                user.RefreshTokens.Add(refreshToken);
                try
                {
                    await _userManager.UpdateAsync(user);
                }
                catch (Exception ex)
                {
                    return new AccoutResultDTO { Massage = ex.Message };
                }
            }
            return new AccoutResultDTO
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(JwtToken),
                RefreshToken = refreshToken.Token,
                RefreshTokenExpired = refreshToken.ExpiredOn
            };
        }

        public async Task<AccoutResultDTO> RegisteSeller(RegisteSellerDTO sellerDTO)
        {
            if (await _userManager.FindByEmailAsync(sellerDTO.Email) is not null) {
                return new AccoutResultDTO() { Massage = "Email is used." };
            }
            if (await _userManager.FindByNameAsync(sellerDTO.UserName) is not null)
            {
                return new AccoutResultDTO() { Massage = "Username is user." };
            }
            var seller = new Seller(sellerDTO);
            var refreshToken = new RefreshToken();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(seller, sellerDTO.Password);
                    if (!result.Succeeded)
                    {
                        var error = "";
                        foreach (var item in result.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AccoutResultDTO() { Massage = error };
                    }
                    var roleResult = await _userManager.AddToRoleAsync(seller, "User");
                    if (!roleResult.Succeeded)
                    {
                        var error = "";
                        foreach (var item in roleResult.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AccoutResultDTO() { Massage = error };
                    }
                    refreshToken=GenerateRefreshToken();
                    seller.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(seller);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return new AccoutResultDTO { Massage = ex.Message };
                }
            }
            var token =await CreateToken(seller);
            return new AccoutResultDTO() { IsAuthenticated=true, Token = new JwtSecurityTokenHandler().WriteToken(token),
                RefreshToken=refreshToken.Token,RefreshTokenExpired=refreshToken.ExpiredOn };
        }
        public async Task<AccoutResultDTO> RegisteUser(RegisteUserDTO UserDTO)
        {
            if (await _userManager.FindByEmailAsync(UserDTO.Email) is not null)
            {
                return new AccoutResultDTO() { Massage = "Email is used." };
            }
            if (await _userManager.FindByNameAsync(UserDTO.UserName) is not null)
            {
                return new AccoutResultDTO() { Massage = "Username is user." };
            }
            var user = new User(UserDTO);
            var refreshToken=new RefreshToken();
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var result = await _userManager.CreateAsync(user, UserDTO.Password);
                    if (!result.Succeeded)
                    {
                        var error = "";
                        foreach (var item in result.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AccoutResultDTO() { Massage = error };
                    }
                    var roleResult = await _userManager.AddToRoleAsync(user, "Seller");
                    if (!roleResult.Succeeded)
                    {
                        var error = "";
                        foreach (var item in roleResult.Errors)
                        {
                            error += " " + item.Description;
                        }
                        return new AccoutResultDTO() { Massage = error };
                    }
                    refreshToken=GenerateRefreshToken();
                    user.RefreshTokens.Add(refreshToken);
                    await _userManager.UpdateAsync(user);
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    return new AccoutResultDTO() { Massage = ex.Message };
                }
            }
            var token = await CreateToken(user);
            return new AccoutResultDTO() { IsAuthenticated = true, Token = new JwtSecurityTokenHandler().WriteToken(token),
            RefreshToken=refreshToken.Token,RefreshTokenExpired=refreshToken.ExpiredOn};
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken()
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiredOn = DateTime.UtcNow.AddDays(1),
                CreatedOn = DateTime.UtcNow
            };
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser user)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Name),
                new Claim(JwtRegisteredClaimNames.Email,user.Email),
                new Claim(JwtRegisteredClaimNames.Jti,new Guid().ToString())
            };
            var roles=await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            SigningCredentials credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken myToken = new JwtSecurityToken(issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudiance"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(10),
                signingCredentials: credentials);
            return myToken;
        }

        public async Task<AccoutResultDTO> CheckRefreshTokenAndRevoke(string token)
        {
            var user=_userManager.Users.SingleOrDefault(x=>x.RefreshTokens.Any(t=>t.Token==token));
            if (user == null)
            {
                return new AccoutResultDTO() { Massage = "not valid token" };
            }
            var refreshToken=user.RefreshTokens.Single(x=>x.Token==token);
            if (!refreshToken.IsActive)
            {
                return new AccoutResultDTO() { Massage="not active"};
            }
            refreshToken.RevokedOn = DateTime.UtcNow;
            var newRefreshToken = GenerateRefreshToken();
            user.RefreshTokens.Add(newRefreshToken);
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch(Exception ex)
            {
                return new AccoutResultDTO { Massage=ex.Message};
            }
            var newToken =await CreateToken(user);
            return new AccoutResultDTO()
            {
                IsAuthenticated = true,
                RefreshToken = newRefreshToken.Token,
                RefreshTokenExpired = newRefreshToken.ExpiredOn,
                Token = new JwtSecurityTokenHandler().WriteToken(newToken)
            };
        }

        public async Task<bool> RevokeRefreshToken(string token)
        {
            var user=_userManager.Users.SingleOrDefault(t=>t.RefreshTokens.Any(t=>t.Token==token));
            if(user is null)
            {
                return false;
            }
            var refreshToken=user.RefreshTokens.Single(t=>t.Token==token);
            if(!refreshToken.IsActive)
            {
                return false;
            }
            refreshToken.ExpiredOn= DateTime.UtcNow;
            try
            {
                await _userManager.UpdateAsync(user);
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
