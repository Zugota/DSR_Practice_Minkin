using DSRPractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Cryptography;
using System.Text;
using DSRPractice.Models;
using System.Collections;

namespace DSR_Practice.Pages.Employees
{
    public class EmployeesModel : PageModel
    {
        public readonly IEmployeeRepository _db;

        public EmployeesModel(IEmployeeRepository db)
        {
            _db = db;
        }

        public IEnumerable<Employee> Employees { get; set; }

        public string all;

        [BindProperty(SupportsGet = true)]
        public string SearchTerm { get; set; }

        public void OnGet()
        {
            Employees = _db.GetAllEmployees();
            /*var list = Employees.ToList();
            StringBuilder sb = new StringBuilder();
            foreach (var i in list)
            {
                sb.Append(i.Name).Append(" ");
            }
            all = sb.ToString();*/

            Employees = _db.Search(SearchTerm);

        }

    }
}
