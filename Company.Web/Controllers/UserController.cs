using Company.Data.Entities;
using Company.Data.Migrations;
using Company.Services.Interfaces.Department.Dto;
using Company.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Company.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger _logger;

        public UserController(UserManager<ApplicationUser> userManager , ILogger<UserController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }
        public async Task<IActionResult> Index(string searchInp)
        {
            List<ApplicationUser> users;
            if (string.IsNullOrEmpty(searchInp))
            {
                users = await _userManager.Users.ToListAsync();

            }
            else
            {
                users = await _userManager.Users
                    .Where(user => user.NormalizedEmail.Trim().Contains(searchInp.Trim().ToUpper()))
                    .ToListAsync();
            }
            return View(users);
        }

        public async Task<IActionResult> Details(string? id, string viewName = "Details")
        {
            var user =  await _userManager.FindByIdAsync(id);
            if (user is null)
                return NotFound();

            if(viewName == "Update")
            {
                var userViewModel = new UserUpdateViewModel
                {
                    Id = user.Id,
                    UserName = user.UserName
                };

                return View(viewName, userViewModel);

            }

            return View(viewName, user);

        }
        public async Task<IActionResult> Update(string id)
        {
            if (id is null)
                return BadRequest();

            return await Details(id, "Update");
        }
        [HttpPost]
        public async Task<IActionResult> Update(string id, UserUpdateViewModel applicationUser)
        {
            if (id != applicationUser.Id)
            {
                _logger.LogError("ID mismatch: Route ID does not match ViewModel ID");
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var user = await _userManager.FindByIdAsync(id);
                    if (user is null)
                    {
                        _logger.LogError($"User with ID {id} not found.");
                        return NotFound();
                    }
                    user.UserName = applicationUser.UserName;
                    user.NormalizedUserName = applicationUser.UserName.ToUpper();

                    var result = await _userManager.UpdateAsync(user);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("User Updated Successfully");
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
            return View(applicationUser);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (id is null)
                    return NotFound();

                var result = await _userManager.DeleteAsync(user);

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
    }
}
