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

        [BindProperty]
        public Employee Employee { get; set; }

        [BindProperty]
        public bool Notify { get; set; }

        public string Message { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id.HasValue) {
                Employee = _employeeRepository.GetEmployee(id.Value);
            }
            else
            {
                Employee = new Employee();
            }
            
            if (Employee == null)
            {
                return RedirectToPage("/Employees/Employees");
            }

            return Page();
        }

        public IActionResult OnPost(int id)
        {
            if (ModelState.IsValid)
            {
                if (Employee.Id > 0)
                {
                    Employee = _employeeRepository.Update(Employee);
                    TempData["SuccessMessage"] = $"Update {Employee.Name} successful";
                }
                else {
                    Employee = _employeeRepository.Add(Employee);
                    id = Employee.Id;
                    TempData["SuccessMessage"] = $"Add {Employee.Name} successful";
                }
                
                return Redirect($"/Employees/Details/{id}/");
            }
            return Page();
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
