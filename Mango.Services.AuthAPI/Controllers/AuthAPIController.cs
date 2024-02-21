using Mango.Services.AuthAPI.Models.Dto;
using Mango.Services.AuthAPI.Service;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
	[ApiController]
	[Route("api/auth")]
	public class AuthAPIController : ControllerBase
	{
		public readonly IAuthService _authService;
		public ResponseDto _responseDto;
		public AuthAPIController(IAuthService authService)
		{
			_authService = authService;
			_responseDto = new ResponseDto();
		}

		[HttpPost("register")]
		public async Task<IActionResult> Register([FromBody] RegisterationRequestDto registerationRequestDto)
		{
			var errorMessage = await _authService.Register(registerationRequestDto);
			if (!String.IsNullOrEmpty(errorMessage))
			{
				_responseDto.IsSuccess = false;
				_responseDto.Msg = errorMessage;
				return BadRequest(_responseDto);
			}
			return Ok(_responseDto);
		}
		[HttpPost("Login")]
		public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
		{
			var loginResponse = await _authService.Login(loginRequestDto);
			if (loginResponse.ApplicationUser is null)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Msg = "Username or password is incorrect";
				return BadRequest(_responseDto);
			}

			_responseDto.Result = loginResponse;
			return Ok(_responseDto);
		}

		[HttpPost("AssignRole")]
		public async Task<IActionResult> AssignRole([FromBody] RegisterationRequestDto registerationRequestDto)
		{
			var assignRoleSuccessful = await _authService.AssignRole(registerationRequestDto.Email, registerationRequestDto.Role.ToUpper());
			if (!assignRoleSuccessful)
			{
				_responseDto.IsSuccess = false;
				_responseDto.Msg = "Error encoutered";
				return BadRequest(_responseDto);
			}

			return Ok(_responseDto);
		}
	}
}
