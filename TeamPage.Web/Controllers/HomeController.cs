using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamPage.Web.Data;
using Microsoft.EntityFrameworkCore;
using TeamPage.Web.Models;
using TeamPage.Web.BLL.Users;
using Microsoft.AspNetCore.Authorization;

namespace TeamPage.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IUserService _userService;

        public HomeController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userService.GetCurrentUserAsync();
                if (user.WorksForExactlyOneCompany())
                {
                    var agency = user.Agencies?.FirstOrDefault();
                    if (agency != null)
                    {
                        return RedirectToAction("Detail", "Agency", new { id = agency.UniqueCode });
                    }
                    var client = user.Clients?.FirstOrDefault();
                    if (client != null)
                    {
                        return RedirectToAction("Detail", "Client", new { id = client.ClientId });
                    }
                }
                else if (user.WorksForMultipleCompanies())
                {
                    return View("Index_LoggedIn_MultipleCompanies", user);
                }
                else
                {
                    return View("Index_LoggedIn_NoCompanies", user);
                }
            }
            
            return View();
        }

        

        [Route("/error")]
        public IActionResult Error()
        {
            return View();
        }
    }
}
