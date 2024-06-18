using Microsoft.AspNetCore.Identity;
using MoviesApiProjeect.Core.Interfaces;
using MoviesApiProjeect.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using MoviesApiProjeect.Core.Const;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using MoviesApiProjeect.Core.Helpers;
using Microsoft.Extensions.Options;

namespace MoviesApiProjeect.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;
        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper,IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
            _jwt = jwt.Value;
        }
        public async Task<AuthModel> RegisterAsync(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthModel { Message = "Email is already registerd!" };
            if (await _userManager.FindByNameAsync(model.UserName) is not null)
                return new AuthModel { Message = "UserName is already registerd!" };
            var user = _mapper.Map<ApplicationUser>(model);
            var result= await _userManager.CreateAsync(user,model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach(var error in result.Errors)
                {
                    errors += $"{error.Description}, ";
                }
                return new AuthModel { Message = errors};
            }
            await _userManager.AddToRoleAsync(user,Roles.User.ToString());
            var jwtSecurityToken=await CreateJwtToken(user);

            return new AuthModel
            {
                Email = user.Email,
                UserName = user.UserName,
                IsAuthenticated = true,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = new List<string> { Roles.User.ToString() },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken)
            };

        }
        public async Task<AuthModel> GetTokenAsync(TokenRequistModel model)
        {
            var authModel=new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if(user is null ||! await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                return authModel;
            }
            var roles=await _userManager.GetRolesAsync(user);
            var jwtSecurityToken = await CreateJwtToken(user);
            authModel.Email = user.Email;
            authModel.ExpiresOn = jwtSecurityToken.ValidTo;
            authModel.UserName = user.UserName;
            authModel.IsAuthenticated = true;
            authModel.Roles=roles.ToList();
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return authModel;
        }
        public async Task<string> AddToRoleAsync(AddToRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.userId);
            var role = await _roleManager.RoleExistsAsync(model.Role);
            if (user is null || !role)
                return "Invalid userId or Role!";
            if (await _userManager.IsInRoleAsync(user, model.Role))
                return "User already in this role!";
            var result= await _userManager.AddToRoleAsync(user, model.Role);
            if (!result.Succeeded)
                return "Somthing went wrong!";
            return string.Empty;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims=await _userManager.GetClaimsAsync(user);
            var userRoles=await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();
            foreach(var role in userRoles)
            {
                roleClaims.Add(new Claim("roles", role));
            }
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);
            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer:_jwt.Issuer,
                audience:_jwt.Audience,
                signingCredentials: signingCredentials,
                claims:claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays)
                );
            return jwtSecurityToken;
        }
    }
}