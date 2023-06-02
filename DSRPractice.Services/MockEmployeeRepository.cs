using DSRPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPractice.Services
{
	public class MockEmployeeRepository : IEmployeeRepository
	{
		private List<Employee> _employeeList;

		public MockEmployeeRepository()
		{
			_employeeList = new List<Employee>()
			{
				new Employee()
				{
					Id = 0, Name = "Ivan", Email = "ivan@mail.ru", PhotoPath = @"\images\avatar.png", Department = Dept.Manager
				},
				new Employee()
				{
					Id = 1, Name = "Petr", Email = "petr@mail.ru", Department = Dept.None
				},
                new Employee()
                {
                    Id = 2, Name = "Alexandr", Email = "admin@mail.ru", Department = Dept.Director
                }
            };
		}

		public IEnumerable<Employee> GetAllEmployees()
		{
			return _employeeList;
		}
	}
}
