using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TeamPage.Web.BLL.Clients;
using Microsoft.AspNetCore.Authorization;

namespace TeamPage.Web.Controllers
{
    [Authorize]
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet("clients")]
        public async Task<IActionResult> Index()
        {
            var clients = await _clientService.GetClientsAsync();
            if (clients != null && clients.Length == 1)
            {
                return RedirectToAction("Detail", new { id = clients[0].Id });
            }
            return View(clients);
        }

        [HttpGet("clients/{id}")]
        public async Task<IActionResult> Detail(int id)
        {
            var client = (await _clientService.GetClientsAsync())?.FirstOrDefault(c => c.Id == id);
            return View(client);
        }

        [HttpGet("agencies/{id}/register-client")]
        public IActionResult Register(string id = null)
        {
            return View(new RegisterClient { AgencyUniqueCode = id });
        }

        [HttpPost("agencies/{id}/register-client")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterClient cmd, string id = null)
        {
            if (ModelState.IsValid)
            {
                await _clientService.RegisterClientAsync(cmd);
                return RedirectToAction("Index", "Home");
            }
            return View(cmd);
        }
    }
}