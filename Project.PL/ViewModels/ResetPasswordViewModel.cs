using System.ComponentModel.DataAnnotations;

namespace Project.PL.ViewModels
{
	public class ResetPasswordViewModel
	{
		[Required(ErrorMessage = "Password is Required")]
		[MinLength(5, ErrorMessage = "Minimum Password Lenght is 5")]
		[DataType(DataType.Password)]
		public string NewPassword { get; set; }


		[Required(ErrorMessage = "Confirm Password is Required")]
		[DataType(DataType.Password)]
		[Compare(nameof(NewPassword), ErrorMessage = "Confirm password doesn't matchh password")]
		public string ConfirmPassword { get; set; }
	}
}
