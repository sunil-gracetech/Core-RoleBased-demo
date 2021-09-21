using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreRoleBased.Models.ViewModel
{
    public class EditViewRoleModel
    {
        public EditViewRoleModel()
        {
            Users = new List<string>();
        }
        public string RoleId { get; set; }
        public string Role { get; set; }
        public List<string> Users { get; set; }
    }
}
