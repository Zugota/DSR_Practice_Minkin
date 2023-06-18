using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DSR_Practice_Debts.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
            var users = _usersContext.Users;
            return View(users.ToList());
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

        public async Task<IActionResult> ShowDebtsList()
        {
            string email = User.Identity.Name;

            var FindidUs = await _usersContext.Users
                .FirstOrDefaultAsync(x => x.Email == email);

            int id = FindidUs.Id;

            /*string _idUs = HttpContext.User.FindFirst("Id").Value;
            int id = Convert.ToInt32(_idUs);*/

            var debts = await _usersContext.Debts.FirstOrDefaultAsync(x => x.userId == id);

            var sqldebts = _usersContext.Debts
                .FromSqlRaw<Debt>("SELECT * FROM Debts WHERE userId = {0}", id)
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

            return RedirectToAction(nameof(Index));

        }

        [HttpGet]
        public async void ShowSumm(Debt debt)
        {
            int summ = 0;
            //for (int i = 0; i < )

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
