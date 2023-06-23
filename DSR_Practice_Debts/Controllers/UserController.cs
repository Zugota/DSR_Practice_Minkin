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
            //var user = _usersContext.Users.FirstOrDefault(x => x.Id == 1);
            string email = User.Identity.Name;

            var FindidUs = _usersContext.Users
                .FirstOrDefault(x => x.Email == email);
            int id = FindidUs.Id;
            return View(_usersContext.Users.FirstOrDefault(x => x.Id == id));
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
            /*if (email == "admin")
            {
                var sqldebtsAdmin = _usersContext.Debts
               .FromSqlRaw<Debt>("UPDATE Debts SET Status = 'Просрочен' WHERE CAST(Debts.DateOfEnd AS datetime2) < CAST(GETDATE() AS smalldatetime) AND Debts.Status NOT LIKE '%Погашен%';" +
               " SELECT * FROM Debts;")
               .ToList();


                return View(sqldebtsAdmin);
            }*/

            var FindidUs = await _usersContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            int id = FindidUs.Id;

            /*string _idUs = HttpContext.User.FindFirst("Id").Value;
            int id = Convert.ToInt32(_idUs);*/

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

            

            //var debts = await _usersContext.Debts.FindAsync("userId", id);

            _usersContext.Entry(debts).Reference(i => i.User).Load();

            return View(sqldebts);

            /*var debts = await _debtContext.Debts
                .Include(x => x.User)
                .FirstOrDefaultAsync(m => m.userId == id);

            return View(debts);*/




            /*string query = "";

            var userDebts = await _usersContext.Debts
                .FromSqlRaw<Debt>("SELECT * FROM Debts WHERE UserId")
                .ToList()
                .FirstOrDefault();*/
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

        /*[HttpGet]
        public IActionResult UpdateDebtsList()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDebtsList(Debt debt)
        {
            string email = User.Identity.Name;

            var FindidUs = await _usersContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            int id = FindidUs.Id;


            //debt.userId = id;


            if ((DateTime.Now - debt.Date).TotalDays < 0)
            {
                debt.Status = "Просрочен";
            }

            _usersContext.Debts.Update(debt);
            await _usersContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }*/

        [HttpGet]
        public async Task<IActionResult> DebtDetails(int id)
        {
            Debt debt = await _usersContext.Debts.FirstOrDefaultAsync(x => x.IdDebt == id);
            return View(debt);
        }

        /*[HttpGet]
        public async Task<IActionResult> PayDebt(int id)
        {
            Debt debt = await _usersContext.Debts.FirstOrDefaultAsync(x => x.IdDebt == id);
            return View(debt);
        }
*/
        /*[HttpPost]
        public async Task<IActionResult> DebtDetails(Debt debt)
        {
            //var debt = await _usersContext.Debts.FirstOrDefaultAsync(x => x.IdDebt == id);

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


            return RedirectToAction(nameof(Index));
        }*/

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

        public async Task<IActionResult> UserC()
        {
            /*string _idUs = HttpContext.User.FindFirst("Id").Value;
            int id = Convert.ToInt32(_idUs);*/

            var user = await _usersContext.Users.FirstOrDefaultAsync(m => m.Email == User.Identity.Name);

            
            return View(user);
        }
    }
}
