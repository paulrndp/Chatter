using Chatter.Data;
using Chatter.Hubs;
using Chatter.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System.Diagnostics;

namespace Chatter.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHubContext<ChatHub> _signalrHub;

        public HomeController(ApplicationDbContext context, UserManager<AppUser> userManager, IHubContext<ChatHub> signalrHub)
        {
            _context = context;
            _userManager = userManager;
            _signalrHub = signalrHub;
        }
        public IActionResult Index()
        {
            var list = _userManager.Users;
            return View(list);

        }
        [HttpGet]
        public IActionResult GetMessages()
        {
            var res = _context.Messages.ToList();
            return Ok(res);
        }
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Username,Text,When,UserId")] Message message)
        {

            message.Username = User.Identity.Name;
            var sender = await _userManager.GetUserAsync(User);
            message.UserId = sender.Id;
            await _context.AddAsync(message);
            await _context.SaveChangesAsync();
            await _signalrHub.Clients.All.SendAsync("Load");
            return View();

        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}