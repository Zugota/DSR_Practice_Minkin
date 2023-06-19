using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using DSR_Practice_Debts.Models;

namespace DSR_Practice_Debts.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly UsersContext _user;

        /*public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }*/

        public HomeController(UsersContext user)
        {
            _user = user;
        }


        public async Task<IActionResult> Index()
        {
            return _user.Users != null ? 
                View(await _user.Users.ToListAsync()) :
                Problem("Entity set 'UniversityContext.Users'  is null.");
        }

        public IActionResult Intro()
        {
            return View();
        }

        [Authorize(Policy = "OnlyForAdmin")]
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