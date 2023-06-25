using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DSR_Practice_Debts.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace DSR_Practice_Debts.Controllers
{
    public class UserController : Controller
    {
        private readonly UsersContext _usersContext;

        public UserController(UsersContext usersContext)
        {
            _usersContext = usersContext;
        }

        public IActionResult Index()
        {
            string email = User.Identity.Name;

            var FindidUs = _usersContext.Users
                .FirstOrDefault(x => x.Email == email);
            int id = FindidUs.Id;
            return View(_usersContext.Users.FirstOrDefault(x => x.Id == id));
        }

        [HttpGet]
        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _usersContext.Users == null)
            {
                return NotFound();
            }

            var user = await _usersContext.Users
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        [Authorize]
        public async Task<IActionResult> ShowDebtsList()
        {
            string email = User.Identity.Name;

            var FindidUs = await _usersContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            int id = FindidUs.Id;


            var debts = await _usersContext.Debts.FirstOrDefaultAsync(x => x.userId == id);
            if (debts == null)
            {
                return RedirectToAction("AddDebt");
            }

            
            //АПДЕЙТ СТАТУСА
            var sqldebts = _usersContext.Debts
                .FromSqlRaw<Debt>("UPDATE Debts SET Status = 'Просрочен' WHERE CAST(Debts.DateOfEnd AS datetime2) < CAST(GETDATE() AS smalldatetime) AND Debts.Status NOT LIKE '%Погашен%';" +
                " SELECT * FROM Debts WHERE userId = {0}", id)
                .ToList();



            int sum = sqldebts.Sum(x => x.Summ);
            _usersContext.Entry(debts).Reference(i => i.User).Load();

            return View(sqldebts);
        }

        [HttpGet]
        [Authorize(Policy = "OnlyForUser")]
        public IActionResult AddDebt()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddDebt(Debt debt)
        {
            string email = User.Identity.Name;

            var FindidUs = await _usersContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            int id = FindidUs.Id;

            
            debt.userId = id;

            debt.Status = "Открыт";
            


            _usersContext.Debts.Add(debt);
            await _usersContext.SaveChangesAsync();

            return RedirectToAction("ShowDebtsList");

        }


        [HttpGet]
        public async Task<IActionResult> DebtDetails(int id)
        {
            Debt debt = await _usersContext.Debts.FirstOrDefaultAsync(x => x.IdDebt == id);
            return View(debt);
        }

        [HttpPost]
        public async Task<IActionResult> PayDebtAcception(Debt debt)
        {
            return View(debt);
        }


        [HttpGet]
        public async Task<IActionResult> PayDebt(int id)
        {
            Debt debt = await _usersContext.Debts.FirstOrDefaultAsync(x => x.IdDebt == id);
            return View(debt);
        }

        [HttpPost]
        public async Task<IActionResult> PayDebt(Debt debt)
        {
            if (debt.DateOfEnd < DateTime.Now)
            {
                int totalDays = (int)(DateTime.Now - debt.DateOfEnd).TotalDays;
                debt.Status = $"Погашен с опозданием на {totalDays} дней";
                debt.RealDateEnd = DateTime.Now;
            }
            else
            {
                debt.Status = "Погашен";
                debt.RealDateEnd = DateTime.Now;
            }
            //debt.Status = "Погашен";
            _usersContext.Update(debt);
            await _usersContext.SaveChangesAsync();
            return RedirectToAction("ShowDebtsList");
        }

        [HttpPost]
        public async Task<IActionResult> DelayDebtAcception(Debt debt)
        {
            return View(debt);
        }

        [HttpPost]
        public async Task<IActionResult> ForgiveDebtAcception(Debt debt)
        {
            return View(debt);
        }

        [HttpPost]
        public async Task<IActionResult> DelayDebt(Debt debt)
        {
            debt.Status = "Отложен";
            _usersContext.Update(debt);
            await _usersContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> ForgiveDebt(Debt debt)
        {
            debt.Status = "Прощён";
            debt.RealDateEnd = DateTime.Now;
            _usersContext.Update(debt);
            await _usersContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}