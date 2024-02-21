namespace Mango.Services.AuthAPI.Models.Dto
{
	public class LoginResponseDto
	{
		public ApplicationUserDto ApplicationUser { get; set; }
		public string Token { get; set; }
	}
}
