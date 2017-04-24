using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TeamPage.Web.BLL.Agencies;

namespace TeamPage.Web.Controllers
{
    [Authorize]
    public class AgencyController : Controller
    {
        private readonly IAgencyService _agencyService;

        public AgencyController(IAgencyService agencyService)
        {
            _agencyService = agencyService;
        }


        [HttpGet("/agencies/{id}")]
        public async Task<IActionResult> Detail(string id)
        {
            var agency = await _agencyService.GetAgencyAsync(id);

            return View(agency);
        }

        [HttpGet("/agency-setup")]
        public IActionResult Setup()
        {
            return View();
        }

        [HttpPost("/agency-setup")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Setup(SetupAgency cmd)
        {
            if (ModelState.IsValid)
            {
                await _agencyService.SetupAgencyAsync(cmd); // ignore return value for now
                return RedirectToAction("Index", "Home");
            }
            return View(cmd);
        }
    }
}