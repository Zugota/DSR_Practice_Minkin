using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using DSR_Practice_Debts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;
using System;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Migrations;
using Humanizer;
using System.Collections.Generic;

namespace DSR_Practice_Debts.Controllers
{
    public class AdminController : Controller
    {
        private readonly UsersContext _usersContext;
        private string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=DSR_Practice_Debts;Integrated Security=True;TrustServerCertificate=true";
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

        [HttpGet]
        public async Task<IActionResult> ImportXML()
        {

            _usersContext.Database.ExecuteSqlRaw("DROP TABLE Debts");
            _usersContext.Database.ExecuteSqlRaw("DROP TABLE Users");


            //_usersContext.Remove(users);
            _usersContext.SaveChanges();

            /*_usersContext.Database.ExecuteSqlRaw("CREATE TABLE Users (" +
                "Id INT NOT NULL PRIMARY KEY IDENTITY(1, 1)," +
                "Email nvarchar(max) NOT NULL," +
                "Password nvarchar(max) NOT NULL)");

            _usersContext.Database.ExecuteSqlRaw("CREATE TABLE Debts (" +
                "IdDebt INT NOT NULL PRIMARY KEY IDENTITY(1, 1)," +
                "Summ INT NOT NULL," +
                "Date datetime2 NOT NULL," +
                "DateOfEnd datetime2 NOT NULL," +
                "Status nvarchar(max) NOT NULL," +
                "userId INT FOREIGN KEY REFERENCES Users(Id)," +
                "RealDateEnd datetime2 NULL)");*/

            _usersContext.Database.ExecuteSql($"CreateUsers");
            _usersContext.Database.ExecuteSql($"CreateDebts");
            User users = new User();

            DataSet dsUsers = new DataSet();
            dsUsers.ReadXml(@"ExportData/usersDB.xml");
            DataTable dtUsers = dsUsers.Tables[0];


            foreach (DataRow row in dtUsers.Rows)
            {
                var cells = row.ItemArray;

                string pas = cells[2].ToString().Replace("'", "''");

                _usersContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT Users ON; INSERT INTO Users (Id, Email, Password) " +
                    $"VALUES ({int.Parse(cells[0].ToString())}, '{(string)cells[1]}', '{pas}')");

            }

            await _usersContext.SaveChangesAsync();

            Debt debts = new Debt();
            DataSet dsDebts = new DataSet();
            dsDebts.ReadXml(@"ExportData/debtsDB.xml");
            DataTable dtDebts = dsDebts.Tables[0];

            foreach (DataRow row in dtDebts.Rows)
            {
                var cells = row.ItemArray;
                string cell6 = cells[6].ToString();
                if (cell6 != "")
                {
                    debts.RealDateEnd = DateTime.Parse(cells[6].ToString());
                }
                else debts.RealDateEnd = null;
                _usersContext.Database.ExecuteSqlRaw($"SET IDENTITY_INSERT Debts ON; INSERT INTO Debts (IdDebt, Summ, Date, DateOfEnd, Status, userId, RealDateEnd) " +
                    $"VALUES ({int.Parse(cells[0].ToString())}, {int.Parse(cells[1].ToString())}, '{DateTime.Parse(cells[2].ToString())}', '{DateTime.Parse(cells[3].ToString())}', " +
                    $"'{(string)cells[4]}', {int.Parse(cells[5].ToString())}, '{cell6}')");


            }
            await _usersContext.SaveChangesAsync();


            return RedirectToAction("Index");
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
