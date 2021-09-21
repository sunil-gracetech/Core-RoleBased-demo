using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CoreRoleBased.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        [Authorize(Roles ="admin,customer")]
        public IActionResult Index()
        {
            return View();
        }
    }
}