using Mango.Services.AuthAPI.Data;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace Mango.Services.AuthAPI.Service
{
	public class AuthService : IAuthService
	{
		private readonly AppDbContext _appDbContext;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IJwtTokenGenerator _jwtTokenGenerator;
		public AuthService(AppDbContext appDbContext, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
		{
			_appDbContext = appDbContext;
			_userManager = userManager;
			_roleManager = roleManager;
			_jwtTokenGenerator = jwtTokenGenerator;
		}

		public async Task<bool> AssignRole(string email, string roleName)
		{
			var user = _appDbContext.ApplicationUsers.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
			if (user is not null)
			{
				if (!_roleManager.RoleExistsAsync(roleName).GetAwaiter().GetResult())
				{
					//create role if does not exist
					_roleManager.CreateAsync(new IdentityRole(roleName)).GetAwaiter().GetResult();
				}
				await _userManager.AddToRoleAsync(user, roleName);
				return true;
			}
			return false;
		}

		public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
		{
			var user = _appDbContext.ApplicationUsers.FirstOrDefault(u=>u.Email.ToLower()==loginRequestDto.Email.ToLower());
			bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
			if (user is null || !isValid)
			{
				return new LoginResponseDto() { ApplicationUser = null, Token = "" };
			}

			//if user was found, generate JWT token
			ApplicationUserDto userDto = new ApplicationUserDto()
			{
				Id = user.Id,
				UserName = user.UserName,
				Email = user.Email,
				phoneNumber = user.PhoneNumber
			};

			var roles = await _userManager.GetRolesAsync(user);
			string Token = _jwtTokenGenerator.GenerateToken(user, roles);

			LoginResponseDto loginResponseDto = new LoginResponseDto()
			{
				ApplicationUser = userDto,
				Token = Token
			};

			return loginResponseDto;
		}

		public async Task<string> Register(RegisterationRequestDto registerationRequestDto)
		{
			ApplicationUser applicationUser = new ApplicationUser() {
				UserName = registerationRequestDto.UserName,
				Email = registerationRequestDto.Email,
				NormalizedEmail = registerationRequestDto.Email.ToUpper(),
				PhoneNumber = registerationRequestDto.phoneNumber
			};

			try
			{
				var result = await _userManager.CreateAsync(applicationUser,registerationRequestDto.Password);
				if (result.Succeeded)
				{
					var userToReturn = _appDbContext.ApplicationUsers.First(u => u.Email == registerationRequestDto.Email);
					ApplicationUserDto applicationUserDto = new ApplicationUserDto()
					{
						Id = userToReturn.Id,
						Email = userToReturn.Email,
						UserName = userToReturn.UserName,
						phoneNumber = userToReturn.PhoneNumber
					};

					return "";
				}
				else
				{
					return result.Errors.FirstOrDefault().Description;
				}
			}catch (Exception ex)
			{

			}
			return "Error Encoutered";
		}
	}
}
