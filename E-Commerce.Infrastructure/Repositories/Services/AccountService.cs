using AutoMapper;
using Azure;
using E_Commerce.Domain.DTOs;
using E_Commerce.Domain.Entities;
using E_Commerce.Infrastructure.Repositories.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static StackExchange.Redis.Role;

namespace E_Commerce.Infrastructure.Repositories.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;


        public AccountService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration _configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.userManager = userManager;
            _roleManager = roleManager;
            this._configuration = _configuration;
            this._httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }
        public async Task<AuthModel> RegisterAsync(RegisterDto model)
        {
            if(await userManager.FindByEmailAsync(model.Email) != null)
            {
                return new AuthModel {Message="Email is already Registered!" };

            }
            if (await userManager.FindByNameAsync(model.Username) != null)
            {
                return new AuthModel { Message = "Username is already Registered!" };

            }
            var user=new ApplicationUser
            { 
                UserName = model.Username,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
            
            };
           var result= await userManager.CreateAsync(user,model.Password);
            if(!result.Succeeded)
            {
                var errors=string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description} , ";
                }
                return new AuthModel { Message = errors };
            }
            await userManager.AddToRoleAsync(user, "User");
            var Token= await CreateToken (user);
            return new AuthModel
            {
                Email = user.Email,
               // TokenExpiration = Token.ValidTo,
                IsAuthenticated = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(Token),
                UserName = user.UserName,
            
            };
        }
        public async Task<AuthModel> LoginAsync(LoginDto model)
        {
            var authModel = new AuthModel();
            var user= await userManager.FindByEmailAsync(model.Email);
            if(user == null||!await userManager.CheckPasswordAsync(user,model.Password))
            {
                authModel.Message="Email or Password is incorrect";
                return authModel;
            }
            var Token = await CreateToken(user);
            var roles = await userManager.GetRolesAsync(user);

            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(Token);
           // authModel.TokenExpiration = Token.ValidTo;
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.Roles = roles.ToList();


            if(user.RefreshTokens.Any(t=>t.IsActive))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActive);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiration = activeRefreshToken.ExpiresOn;
            }
            else
            {
                var RefreshToken = GenerateRefreshToken();
                authModel.RefreshToken = RefreshToken.Token;
                authModel.RefreshTokenExpiration = RefreshToken.ExpiresOn;
                user.RefreshTokens.Add(RefreshToken);
                await userManager.UpdateAsync(user);
            }
            return authModel;
        }
        public async Task<AuthModel> GetCurrentUserAsync()
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;

            // Get the current user from claims
            var user = await userManager.GetUserAsync(userClaim);

            if (user == null)
                return new AuthModel { IsAuthenticated = false, Message = "User not found" };

            var token = await CreateToken(user);

            return new AuthModel
            {
                UserName = user.UserName,
                Email = user.Email,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                IsAuthenticated = true
            };
        }
        public async Task<AddressDto> GetCurrentUserAddressAsync()
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;

            var user = await userManager.GetUserAsync(userClaim);

            if (user == null || user.Address == null)
                return null; // Handle case when user or address is null

            var addressDto = _mapper.Map<AddressDto>(user.Address);
            return addressDto;
        }

        public async Task<AddressDto> UpdateCurrentUserAddressAsync(AddressDto addressDto)
        {
            var userClaim = _httpContextAccessor.HttpContext!.User;

            var user = await userManager.GetUserAsync(userClaim);

            if (user == null)
                throw new Exception("User not found"); 

            
            user.Address = _mapper.Map<Address>(addressDto);

           
            var result = await userManager.UpdateAsync(user);

            if (result.Succeeded)
                return _mapper.Map<AddressDto>(user.Address);

            throw new Exception("Failed to update address"); 
        }

        public async Task<string> AddRoleAsync(AddRoleDto model)
        {
            var user = await userManager.FindByIdAsync(model.UserId);
            if (user == null)
                return "Invaild UserId";

            if (!await _roleManager.RoleExistsAsync(model.Role))
                return "Invaild Role";

            if (await userManager.IsInRoleAsync(user, model.Role))
                return "User already assigned to this Role";

            var result= await userManager.AddToRoleAsync(user, model.Role);
            return result.Succeeded ? string.Empty : "Somthing went Wrong";
        }
        private async Task<JwtSecurityToken> CreateToken(ApplicationUser User)
        {
            var claims = new List<Claim>
                      {
                     new Claim(ClaimTypes.Name, User.UserName),
                     new Claim(ClaimTypes.NameIdentifier, User.Id),
                     new Claim(JwtRegisteredClaimNames.Sub, User.UserName),
                     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                     new Claim(JwtRegisteredClaimNames.Email, User.Email!),
                     new Claim("uid", User.Id)
                     };
            var roles = await userManager.GetRolesAsync(User);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            SecurityKey Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]));
            SigningCredentials signingCred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);
            var Token = new JwtSecurityToken(
                issuer: _configuration["JWT:issuer"],
                audience: _configuration["JWT:audience"],
                claims: claims,
                signingCredentials: signingCred,
                expires: DateTime.Now.AddDays(30)
                );
            return Token;
        }
        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var generator = new RNGCryptoServiceProvider();
            generator.GetBytes(randomNumber);
            return new RefreshToken
            {
                Token =Convert.ToBase64String(randomNumber),
                ExpiresOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow
            };
        }

        //private void SetRefreshTokenInCookie(string refreshToken,DateTime expires)
        //{
        //    var cookieOptions = new CookieOptions
        //    {
        //        HttpOnly = true,
        //        Expires=expires
        //    };
        //    Response.
        //}

    }
}
