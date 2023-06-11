using DSRPractice.Models;
using DSRPractice.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSR_Practice.ViewComponents
{
    public class HeadCountViewComponent : ViewComponent
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HeadCountViewComponent(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IViewComponentResult Invoke(Dept? department = null)
        {
            var result = _employeeRepository.empCountInDept(department);
            return View(result);
        }
    }
}
