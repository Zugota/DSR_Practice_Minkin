using DSRPractice.Models;
using DSRPractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSR_Practice.Pages.Employees
{
    public class EditorModel : PageModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EditorModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public Employee Employee { get; set; }

        public IActionResult OnGet(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
            if (Employee == null)
            {
                return RedirectToPage("/Employees/Employees");
            }

            return Page();
        }

        public IActionResult OnPost(Employee employee, int id)
        {
            Employee = _employeeRepository.Update(employee);
            return Redirect($"/Employees/Details/{id}/");
        }
    }
}
