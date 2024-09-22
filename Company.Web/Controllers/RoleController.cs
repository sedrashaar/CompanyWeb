using Company.Data.Entities;
using Company.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Company.Web.Controllers
    
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<RoleController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public RoleController( RoleManager<IdentityRole> roleManager ,ILogger<RoleController> logger , UserManager<ApplicationUser> userManager )
        {
            _roleManager = roleManager;
            _logger = logger;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return View(roles);
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(RoleViewModel roleModel)
        {
            var role = new IdentityRole
            {
              Name = roleModel.Name
            
            };
            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            foreach (var item in result.Errors)
            {
                _logger.LogError(item.Description);
            }
            return View(roleModel);
        }

        public async Task<IActionResult> Update(string id)
        {
            if (id is null)
                return BadRequest();

            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, RoleViewModel roleUpdate)
        {
            if (id != roleUpdate.Id)
            {
                _logger.LogError("ID mismatch: Route ID does not match ViewModel ID");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role is null)
                    {
                        _logger.LogError($"Role with ID {id} not found.");
                        return NotFound();
                    }
                    role.Name = roleUpdate.Name;
                    role.NormalizedName= roleUpdate.Name.ToUpper();

                    var result = await _roleManager.UpdateAsync(role);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Role Updated Successfully");
                        return RedirectToAction("Index");
                    }
                    foreach (var item in result.Errors)
                    {
                        _logger.LogError(item.Description);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
            return View(roleUpdate);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var role = await _roleManager.FindByIdAsync(id);
                if (id is null)
                    return NotFound();

                var result = await _roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var item in result.Errors)
                {
                    _logger.LogError(item.Description);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role is null)
                return NotFound();

          
                var roleViewModel = new RoleViewModel
                {
                    Id = role.Id,
                    Name = role.Name
                };

                return View(viewName, roleViewModel);


        }

        public async Task<IActionResult> AddorRemoveUsers(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            
            if(role is null)
            {
                return NotFound();
            }
            ViewBag.RoleId = roleId;
            var users = await _userManager.Users.ToListAsync();
            var usersInRole = new List<UserInRoleViewModel>();

            foreach (var user in users) 
            {
              var userInRole = new UserInRoleViewModel
              {
                    UserId = user.Id,
                    UserName = user.UserName
              };

                if (await _userManager.IsInRoleAsync(user, role.Name))
                    userInRole.IsSelected = true;
                else userInRole.IsSelected = false;

                usersInRole.Add(userInRole);
            }
            return View(usersInRole);
        }
        [HttpPost]
        public async Task<IActionResult> AddorRemoveUsers(string roleId, List<UserInRoleViewModel> users)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role is null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                foreach (var user in users)
                {
                    var appUser = await _userManager.FindByIdAsync(user.UserId);
                    if (appUser is not  null)
                    {
                        if (user.IsSelected && !await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.AddToRoleAsync(appUser, role.Name);
                        }
                        else if (!user.IsSelected && await _userManager.IsInRoleAsync(appUser, role.Name))
                        {
                            await _userManager.RemoveFromRoleAsync(appUser, role.Name);
                        }
                    }
                }
                return RedirectToAction("Update", new { id = roleId });

            }
            return View(users);
        }
    }

    
}
