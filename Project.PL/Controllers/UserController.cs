using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project.DAL.Models;
using Project.PL.Helper;
using Project.PL.ViewModels;

namespace Project.PL.Controllers
{
    [Authorize]

    public class UserController : Controller
	{
		private readonly UserManager<ApplicationUser> _usermanager;

		public UserController(UserManager<ApplicationUser> usermanager)
        {
			_usermanager = usermanager;
		}



		public async Task<IActionResult> Index(string searchinput)
		{
			var users = Enumerable.Empty<UserViewModel>();

			if (string.IsNullOrEmpty(searchinput))
			{
				users = await _usermanager.Users.Select(U => new UserViewModel
				{
					Id = U.Id,
					FirstName = U.FirstName,
					LastName = U.LastName,
					Email = U.Email,
					Roles = _usermanager.GetRolesAsync(U).Result
				}).ToListAsync();
			}
			else
			{
				users = await _usermanager.Users.Where(U => U.Email
											   .ToLower()
											   .Contains(searchinput.ToLower()))
											   .Select(U => new UserViewModel
											   {
												   Id = U.Id,
												   FirstName = U.FirstName,
												   LastName = U.LastName,
												   Email = U.Email,
												   Roles = _usermanager.GetRolesAsync(U).Result
											   }).ToListAsync();
			}

			return View(users);
		}


        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest();


			var userfromDb= await _usermanager.FindByIdAsync(id);

            if (userfromDb is null)
                return NotFound();

			var user = new UserViewModel()
			{
				Id = userfromDb.Id,
				FirstName = userfromDb.FirstName,
				LastName = userfromDb.LastName,
				Email = userfromDb.Email,
				Roles = _usermanager.GetRolesAsync(userfromDb).Result
			};
            return View(viewname, user);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, UserViewModel model) //to prevent any edit from anyone
        {
            if (id != model.Id)
                return BadRequest();

                if (ModelState.IsValid) 
                {
                    var userfromDb = await _usermanager.FindByIdAsync(id);

                    if (userfromDb is null)
                        return NotFound();

                    userfromDb.FirstName = model.FirstName; 
                    userfromDb.LastName = model.LastName;
                    userfromDb.Email = model.Email;

                    var user = await _usermanager.UpdateAsync(userfromDb);
                    if (user.Succeeded)
                    return RedirectToAction(nameof(Index));
                }
           
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Delete(string? id)
        {
            return await Details(id, "Delete");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete([FromRoute] string? id, UserViewModel model) //to prevent any edit from anyone
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid) // to can use data validation
            {
                var userfromDb = await _usermanager.FindByIdAsync(id);

                if (userfromDb is null)
                    return NotFound();

                userfromDb.FirstName = model.FirstName;
                userfromDb.LastName = model.LastName;
                userfromDb.Email = model.Email;

                var user = await _usermanager.DeleteAsync(userfromDb);
                if (user.Succeeded)
                    return RedirectToAction(nameof(Index));
            }

            return View();
        }



    }
}
