using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
	public class SignInViewModel
	{
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Email is Invalid")]
		public string Email { get; set; }


		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password Lenght is 5")]
		[DataType(DataType.Password)]
		public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
