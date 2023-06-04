using DSRPractice.Models;
using DSRPractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSR_Practice.Pages.Employees
{
    public class DetailsModel : PageModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        public DetailsModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Employee Employee { get; private set; }

        /*public void OnGet(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
            if (Employee == null)
            {
                return RedirectToPage("/Employees");
            }
            return Page();
        }*/

        public IActionResult OnGet(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
            if (Employee == null)
            {
                return RedirectToPage("/Employees/Employees");
            }
            return Page();
        }
    }
}
