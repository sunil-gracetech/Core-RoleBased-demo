using CoreRoleBased.Models.ViewModel;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreRoleBased.Repository
{
   public interface IUser
    {
         Task<IdentityResult> CreateUser(SignUpViewModel model);
         Task<SignInResult> AuthenticateUser(SignInViewModel model);
         void SignOut();
         Task<IdentityResult> CreateRole(RoleViewModel roleView);
        IQueryable<IdentityRole> GetAllRole();
        Task<EditViewRoleModel> EditRole(string id);
        Task<IdentityResult> UpdateRole(EditViewRoleModel model);
        Task<List<UserRoleViewModel>> EditUserInRole(string id);
        Task<IdentityResult> EditUsersInRole(List<UserRoleViewModel> model, string roleId);
        IQueryable<IdentityUser> GetAllUsers();
        Task<IdentityResult> DeleteUser(string id);
       Task<IdentityUser> GetUserById(string id);
    }
}
