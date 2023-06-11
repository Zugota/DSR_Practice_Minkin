using DSRPractice.Models;
using DSRPractice.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DSR_Practice.Pages.Employees
{
    public class DeleteModel : PageModel
    {
        private readonly IEmployeeRepository _employeeRepository;

        [BindProperty]
        public Employee Employee { get; set; }
        
        public DeleteModel(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }
        
        public IActionResult OnGet(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
            if (Employee == null) {
                return RedirectToPage("/Employees/Employees");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            Employee delEmployee = _employeeRepository.Delete(Employee.Id);
            if (delEmployee == null)
            {
                return RedirectToPage("/Employees/Employees");
            }
            return RedirectToPage("/Employees/Employees");
        }
    }
}