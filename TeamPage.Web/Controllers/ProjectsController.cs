using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace TeamPage.Web.Controllers
{
    public class ProjectsController : Controller
    {
        [HttpGet("/projects")]
        public IActionResult Index()
        {
            return View();
        }
    }
}