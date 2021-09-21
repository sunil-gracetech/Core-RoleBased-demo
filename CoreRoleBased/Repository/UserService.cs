using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoreRoleBased.Models;
using CoreRoleBased.Models.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace CoreRoleBased.Repository
{
    public class UserService : IUser
    {
        private readonly UserManager<IdentityUser> applicationContext;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserService(UserManager<IdentityUser> applicationContext,
            SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            this.applicationContext = applicationContext;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }
        public async Task<SignInResult> AuthenticateUser(SignInViewModel model)
        {
            var user = applicationContext.Users.SingleOrDefault(e => e.Email == model.Email);
            var result = await signInManager.PasswordSignInAsync(model.Email, model.Password, true, false);
            return result;

        }

        public async void SignOut()
        {
            await signInManager.SignOutAsync();
        }

        public async Task<IdentityResult> CreateUser(SignUpViewModel model)
        {
            var user = new IdentityUser()
            {
                Email = model.Email,
                PasswordHash = model.Password,
                UserName = model.Email,
                PhoneNumber = model.Mobile
            };
            var result = await applicationContext.CreateAsync(user, model.Password);
            return result;
        }

        public async Task<IdentityResult> CreateRole(RoleViewModel roleView)
        {
            var user = new IdentityRole()
            {
                Name = roleView.Role
            };
            var result = await roleManager.CreateAsync(user);
            return result;
        }
        public IQueryable<IdentityRole> GetAllRole()
        {
            return roleManager.Roles;
        }

        public async Task<EditViewRoleModel> EditRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var rolemodel = new EditViewRoleModel
            {
                Role = role.Name,
                RoleId = role.Id
            };
            foreach (IdentityUser usr in applicationContext.Users.ToList())
            {
                bool r = await applicationContext.IsInRoleAsync(usr, role.Name);
                if (r == true)
                {
                    rolemodel.Users.Add(usr.UserName);
                }
            }
            return rolemodel;
        }

        public async Task<IdentityResult> UpdateRole(EditViewRoleModel model)
        {
            var user = await roleManager.FindByIdAsync(model.RoleId);
            user.Name = model.Role;
            var result = await roleManager.UpdateAsync(user);
            return result;
        }

        public async Task<List<UserRoleViewModel>> EditUserInRole(string id)
        {
            var role = await roleManager.FindByIdAsync(id);
            var list = new List<UserRoleViewModel>();
            foreach (var user in applicationContext.Users.ToList())
            {
                UserRoleViewModel userRole = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };
                if (await applicationContext.IsInRoleAsync(user, role.Name))
                {
                    userRole.IsSelected = true;
                }
                else
                {
                    userRole.IsSelected = false;
                }
                list.Add(userRole);
            }

            return list;
        }

        public async Task<IdentityResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId)
        {
            var role = await roleManager.FindByIdAsync(roleId);


            IdentityResult result = null;
            for (int i = 0; i < model.Count; i++)
            {
                var user = await applicationContext.FindByIdAsync(model[i].UserId);



                if (model[i].IsSelected && !(await applicationContext.IsInRoleAsync(user, role.Name)))
                {
                    result = await applicationContext.AddToRoleAsync(user, role.Name);
                }
                else if (!model[i].IsSelected && await applicationContext.IsInRoleAsync(user, role.Name))
                {
                    result = await applicationContext.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }
            }
            return result;
        }

        public IQueryable<IdentityUser> GetAllUsers()
        {
            var result = applicationContext.Users;
            return result;
        }

        public async Task<IdentityResult> DeleteUser(string id)
        {
            var user = await applicationContext.FindByIdAsync(id);
            if (user != null)
            {
                var r = await applicationContext.DeleteAsync(user);
                return r;
            }
            else
            {
                return null;
            }
        }

        public async Task<IdentityUser> GetUserById(string id)
        {
            var user = await applicationContext.FindByIdAsync(id);
            return user;
        }
    }
}
