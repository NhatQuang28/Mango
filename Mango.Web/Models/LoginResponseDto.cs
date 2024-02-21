namespace Mango.Web.Models
{
	public class LoginResponseDto
	{
		public ApplicationUserDto ApplicationUser { get; set; }
		public string Token { get; set; }
	}
}
