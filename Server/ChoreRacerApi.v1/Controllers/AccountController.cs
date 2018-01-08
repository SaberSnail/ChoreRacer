using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace ChoreRacerApi.v1.Controllers
{
	[Route("[controller]/[action]")]
	public sealed class AccountController : Controller
	{
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
		{
			m_userManager = userManager;
			m_signInManager = signInManager;
			m_configuration = configuration;
		}

		[HttpPost]
		public async Task<object> Login([FromBody] LoginDto login)
		{
			var result = await m_signInManager.PasswordSignInAsync(login.Email, login.Password, false, false);
			if (result.Succeeded)
			{
				var user = m_userManager.Users.SingleOrDefault(r => r.Email == login.Email);
				return GenerateJwtToken(login.Email, user);
			}

			throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
		}

		[HttpPost]
		public async Task<object> Register([FromBody] RegisterDto register)
		{
			var user = new IdentityUser
			{
				UserName = register.Email,
				Email = register.Email,
			};
			var result = await m_userManager.CreateAsync(user, register.Password);
			if (result.Succeeded)
			{
				await m_signInManager.SignInAsync(user, false);
				return GenerateJwtToken(register.Email, user);
			}

			throw new ApplicationException("UNKNOWN_ERROR");
		}

		private string GenerateJwtToken(string email, IdentityUser user)
		{
			var claims = new List<Claim>
			{
				new Claim(JwtRegisteredClaimNames.Sub, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				new Claim(ClaimTypes.NameIdentifier, user.Id),
			};
			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(m_configuration["JwtKey"]));
			var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
			var expires = DateTime.Now.AddDays(Convert.ToDouble(m_configuration["JwtExpireDays"]));
			var token = new JwtSecurityToken(
				m_configuration["JwtIssuer"],
				m_configuration["JwtIssuer"],
				claims,
				expires: expires,
				signingCredentials: credentials);
			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		readonly SignInManager<IdentityUser> m_signInManager;
		readonly UserManager<IdentityUser> m_userManager;
		readonly IConfiguration m_configuration;
	}
}