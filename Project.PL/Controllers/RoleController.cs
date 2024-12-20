using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Project.DAL.Models;
using Project.PL.ViewModels;
using Microsoft.EntityFrameworkCore;
using Project.PL.Helper;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;

namespace Project.PL.Controllers
{
    [Authorize]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController(RoleManager<IdentityRole> roleManager,UserManager<ApplicationUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }



        public async Task<IActionResult> Index(string searchinput)
        {
            var roles = Enumerable.Empty<RoleViewModel>();

            if (string.IsNullOrEmpty(searchinput))
            {
                roles = await _roleManager.Roles.Select( R => new RoleViewModel()
                {
                    Id = R.Id,
                    RoleName = R.Name
                }).ToListAsync();
            }
            else
            {
                roles = await _roleManager.Roles.Where(R => R.Name
                                               .ToLower()
                                               .Contains(searchinput.ToLower()))
                                               .Select(R => new RoleViewModel
                                               {
                                                   Id = R.Id,
                                                   RoleName = R.Name
                                               }).ToListAsync();
            }

            return View(roles);
        }


        [HttpGet]
        public IActionResult create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> create(RoleViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = new IdentityRole()
                {
                    Name=model.RoleName
                };
                await _roleManager.CreateAsync(role);
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> Details(string? id, string viewname = "Details")
        {
            if (id is null)
                return BadRequest();


            var rolefromDb = await _roleManager.FindByIdAsync(id);

            if (rolefromDb is null)
                return NotFound();

            var role = new RoleViewModel()
            {
                Id = rolefromDb.Id,
                RoleName = rolefromDb.Name
            };
            return View(viewname, role);
        }




        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            return await Details(id, "Edit");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromRoute] string? id, RoleViewModel model) //to prevent any edit from anyone
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid) // to can use data validation
            {
                var rolefromDb = await _roleManager.FindByIdAsync(id);

                if (rolefromDb is null)
                    return NotFound();

                rolefromDb.Name = model.RoleName;

                var user = await _roleManager.UpdateAsync(rolefromDb);
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
        public async Task<IActionResult> Delete([FromRoute] string? id, RoleViewModel model) //to prevent any edit from anyone
        {
            if (id != model.Id)
                return BadRequest();

            if (ModelState.IsValid) // to can use data validation
            {
                var rolefromDb = await _roleManager.FindByIdAsync(id);

                if (rolefromDb is null)
                    return NotFound();

                rolefromDb.Name = model.RoleName;

                var user = await _roleManager.DeleteAsync(rolefromDb);
                if (user.Succeeded)
                    return RedirectToAction(nameof(Index));
            }

            return View();
        }


        [HttpGet]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            ViewData["RoleId"] = roleId;

            var UserInRole = new List<UserInRoleViewModel>();
            var users = await _userManager.Users.ToListAsync();

            foreach (var user in users)
            {
                var userinrole = new UserInRoleViewModel()
                {
                    UserName = user.UserName,
                    UserId = user.Id,
                };
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    userinrole.IsSelected = true;
                }
                else
                {
                    userinrole.IsSelected = false;
                }
                UserInRole.Add(userinrole);
            }
            return View(UserInRole);

        }



        [HttpPost]
        public async Task<IActionResult> AddOrRemoveUsers(string roleId,List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role is null)
                return NotFound();

            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appuser = await _userManager.FindByIdAsync(user.UserId);
                    if(appuser is not null)
                    {
                        if (user.IsSelected && ! await _userManager.IsInRoleAsync(appuser,role.Name))
                        {
                            await _userManager.AddToRoleAsync(appuser, role.Name);
                        }
                        else if (! user.IsSelected && await _userManager.IsInRoleAsync(appuser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appuser, role.Name);
                        }
                    }
                    
                }
                return RedirectToAction(nameof(Edit),new { id = roleId });
            }
            return View();
        }


    }
}
