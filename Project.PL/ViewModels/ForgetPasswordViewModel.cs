using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
	public class ForgetPasswordViewModel
	{
		[Required(ErrorMessage = "Email is Required")]
		[EmailAddress(ErrorMessage = "Email is Invalid")]
		public string Email { get; set; }
	}
}
