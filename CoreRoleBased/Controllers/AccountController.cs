using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRoleBased.Models.ViewModel;
using CoreRoleBased.Repository;
using Microsoft.AspNetCore.Mvc;

namespace CoreRoleBased.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUser user;

        public AccountController(IUser user) {
            this.user = user;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(SignUpViewModel signUpViewModel)
        {
            var result=user.CreateUser(signUpViewModel);
            if(result.Result.Succeeded)
            {
                ViewBag.message = "user created successfully !";
            }
            else
            {
                ViewBag.message = "user not created successfully !";

            }
            return View();
        }

        public ActionResult SignIn()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View("SignIn");
        }
        [HttpPost]
        public IActionResult SignIn(SignInViewModel signInViewModel)
        {
           var result= user.AuthenticateUser(signInViewModel);
            if (result.Result.Succeeded)
            {
                return RedirectToAction("Index", "Product");
            }
            else
            {
                ViewBag.message = "Invalid login !";
              
                return View();
            }
        }

        public IActionResult logout()
        {
            user.SignOut();
            return RedirectToAction("Index");
        }

        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateRole(RoleViewModel model)
        {
            var result = user.CreateRole(model);
            if (result.Result.Succeeded)
            {
                ViewBag.message = "role created successfully";
            }
            else
            {
                foreach(var e in result.Result.Errors)
                {
                    ModelState.AddModelError(string.Empty, e.Description);
                }
            }
            return View();
        }
        public IActionResult GetAllRole()
        {
            var roles = user.GetAllRole();
            return View(roles);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var result = await user.EditRole(id);
            return View(result);
        }

        [HttpPost]
        public IActionResult EditRole(EditViewRoleModel model)
        {
            var result = user.UpdateRole(model);
            if (result.Result.Succeeded)
            {
                return RedirectToAction("GetAllRole");
            }
            else
            {
                ViewBag.message = "Role not updated successfully !";
                return View(result);
            }

        }

        [HttpGet]
        public async Task<IActionResult> EditUserInRole(string id)
        {
            ViewBag.roleId = id;
            var result = await user.EditUserInRole(id);
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> EditUsersInRolePost(List<UserRoleViewModel> model)
        {
           string id=Request.Form["roleId"];
            var result = await user.EditUsersInRole(model, id);
            if (result.Succeeded)
            {
                return RedirectToAction("EditRole", new { id = id });
            }
            else
            {
                return View();
            }
        }

        public IActionResult ListAllUsers()
        {
            return View(user.GetAllUsers());
        }
        public async Task<IActionResult> DeleteUser(string id)
        {
            var r = await user.DeleteUser(id);
            if (r == null)
            {
                ViewBag.message = "User not found with ${id} id .";
            }
            else
            {
                ViewBag.message = "User deleted with ${id} id .";
            }
            return RedirectToAction("ListAllUsers");
        }

        public async Task<IActionResult> EditUser(string id)
        {
            var u =await user.GetUserById(id);
            return View(u);
        }

    }
}