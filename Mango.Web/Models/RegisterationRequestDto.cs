﻿namespace Mango.Web.Models
{
	public class RegisterationRequestDto
	{
		public string UserName { get; set; }
		public string Email { get; set; }
		public string phoneNumber { get; set; }
		public string Password { get; set; }
		public string? Role { get; set; }
	}
}