using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
	public class RegisterationRequestDto
	{
		[Required]
		public string UserName { get; set; }
		[Required]
		public string Email { get; set; }
		[Required]
		public string phoneNumber { get; set; }
		[Required]
		public string Password { get; set; }
		public string? Role { get; set; }
	}
}
