using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DSR_Practice_Debts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace DSR_Practice_Debts.Controllers
{
    public class AdminController : Controller
    {
        private readonly UsersContext _usersContext;

        /*private readonly ILogger<HomeController> _logger;*/

        public AdminController(UsersContext usersContext)
        {
            _usersContext = usersContext;
            /*_logger = logger;*/
        }

        public async Task<IActionResult> Index()
        {
            return _usersContext.Users != null ?
                View(await _usersContext.Users.ToListAsync()) :
                Problem("Entity set 'UniversityContext.Users'  is null.");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ExportXML()
        {
            string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DSR_Practice_Debts;Integrated Security=True;TrustServerCertificate=true";
            string sqlUsers = "SELECT * FROM Users";
            string sqlDebts = "SELECT * FROM Debts";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sqlUsers, connection);

                DataSet ds = new DataSet("Users");
                DataTable dt = new DataTable("User");
                ds.Tables.Add(dt);
                adapter.Fill(ds.Tables["User"]);
                ds.WriteXml(@"ExportData/usersDB.xml");

                SqlDataAdapter adapter2 = new SqlDataAdapter(sqlDebts, connection);
                DataSet ds2 = new DataSet("Debts");
                DataTable dt2 = new DataTable("Debt");
                ds2.Tables.Add(dt2);
                adapter2.Fill(ds2.Tables["Debt"]);

                ds2.WriteXml("ExportData/debtsDB.xml");

                connection.Close();
            }
            return RedirectToAction("Index");
        }

        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
