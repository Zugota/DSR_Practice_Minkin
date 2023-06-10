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

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }

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

            TempData["SuccessMessage"] = $"Update {Employee.Name} successful";

            return Redirect($"/Employees/Details/{id}/");
        }

        public void OnPostUpdateNotification(int id)
        {
            Employee = _employeeRepository.GetEmployee(id);
            if (Notify)
            {
                Message = "Спасибо за включение уведомления!";
            }
            else
            {
                Message = "Вы выключили уведомления";
            }
        }
    }
}
