using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.VisualStudio.Web.CodeGeneration;
using Project.DAL.Models;
using Project.PL.Helper;
using Project.PL.ViewModels;

namespace Project.PL.Controllers
{
	public class AccountController : Controller
    {
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
		#region SignUp
		[HttpGet]
        public IActionResult SignUp()
        {
            return View();
        }

		[HttpPost]
		public async Task <IActionResult> SignUp(SignUpViewModel model)
		{
			if(ModelState.IsValid)
			{
				//to ensure that this is uniqe or not empty
				var user=await _userManager.FindByNameAsync(model.UserName);
				if (user is null) 
				{
					user = await _userManager.FindByEmailAsync(model.Email);
					if (user is null)
					{
						user = new ApplicationUser()
						{
							UserName = model.UserName,
							Email = model.Email,
							FirstName = model.FirstName,
							LastName = model.LastName,
							IsAgree = model.IsAgree
						};
						var result=await _userManager.CreateAsync(user,model.Password);
						if(result.Succeeded)
							return RedirectToAction(nameof(SignIn));
						foreach(var error in result.Errors)
						{
							ModelState.AddModelError(string.Empty, error.Description);
						}
						
					}

				}
				ModelState.AddModelError(string.Empty, "Username is already Exists (;");
			}
			return View();
		}

		#endregion

		#region SignIn
		public IActionResult SignIn() 
		{
			return View(); 
		}

		[HttpPost]
		public async Task<IActionResult> SignIn(SignInViewModel model)
		{
			if(ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if (user is not null)
				{
					var flag= await _userManager.CheckPasswordAsync(user, model.Password);
					if (flag)
					{
						//take user, password, want save password when close browser or not and lock account after many trails
						var result=	await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction(nameof(HomeController.Index), "Home");
						}					
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid Login");
			}
			return View();
		}
		#endregion

		#region SignOut

		public new async Task <IActionResult> SignOut()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction(nameof(SignIn));
		}
        #endregion

        #region ForgetPassword
		public IActionResult ForgetPassword()
		{
			return View();
		}

		public async Task<IActionResult> SendResetPasswordURL(ForgetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var user = await _userManager.FindByEmailAsync(model.Email);
				if(user is not null)
				{
					//URL which send in body

					//send token with URL to be valid for one time or some minutes
					var token = await _userManager.GeneratePasswordResetTokenAsync(user);

					//Create URL which will sent in email
					var URl= Url.Action("ResetPassword", "Account", new { email = model.Email,token=token }, Request.Scheme);


					//Create email
					var Email = new Email()
					{
						Subject = "Reset your Password",
						Receptionists = model.Email,
						Body = URl
					};


					//send Email
					EmailSettings.SendEmail(Email);

					//Redirect to action
					return RedirectToAction(nameof(CheckYourBox));

				}
				ModelState.AddModelError(string.Empty, "This Email Is Invalid");
			}
		
			return View(nameof(ForgetPassword), model);
		}


		public IActionResult CheckYourBox()
		{
			return View();
		}

		[HttpGet]
		public IActionResult ResetPassword(string email,string token)
		{
			TempData["email"] = email; 
			TempData["token"] = token;
			return View();
		}


		[HttpPost]
		public async Task <IActionResult> ResetPassword(ResetPasswordViewModel model)
		{
			if(ModelState.IsValid)
			{
				var email = TempData["email"] as string;
				var token = TempData["token"] as string;

				var user = await _userManager.FindByEmailAsync(email);
				if (user is not null)
				{
					var result= await _userManager.ResetPasswordAsync(user, token,model.NewPassword);
					if(result.Succeeded)
						return RedirectToAction(nameof(SignIn));

					foreach(var error in result.Errors)
					{
						ModelState.AddModelError(string.Empty, error.Description);
					}
				}
				ModelState.AddModelError(string.Empty, "Invalid Reset Password");

			}

			return View(model);
		}
		#endregion


	}
}
