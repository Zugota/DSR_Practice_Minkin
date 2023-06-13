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

        public Employee Add(Employee newEmployee)
        {
            newEmployee.Id = _employeeList.Max(x => x.Id) + 1;
			_employeeList.Add(newEmployee);
			return newEmployee;
        }

        public Employee Delete(int id)
        {
			Employee empDelete = _employeeList.FirstOrDefault(x => x.Id == id);
			if (empDelete != null)
			{
				_employeeList.Remove(empDelete);
			}
			return empDelete;
        }

		public IEnumerable<DeptHeadCount> empCountInDept(Dept? dept)
		{
			IEnumerable<Employee> query = _employeeList;

			if (dept.HasValue)
			{
				query = query.Where(x => x.Department == dept.Value);
			}

			return query.GroupBy(x => x.Department).Select(x =>
											new DeptHeadCount()
											{
												Department = x.Key.Value,
												Count = x.Count()
											}).ToList();
		}

		public IEnumerable<Employee> GetAllEmployees()
		{
			return _employeeList;
		}

		public Employee GetEmployee(int id)
		{
			return _employeeList.FirstOrDefault(x => x.Id == id);
		}

		public IEnumerable<Employee> Search(string term)
		{
			if(string.IsNullOrWhiteSpace(term))
			{
				return _employeeList;
			}
			else
			{
				return _employeeList.Where(x => x.Name.ToLower().Contains(term.ToLower()) || x.Email.Contains(term.ToLower()));
			}
		}

		public Employee Update(Employee updatedEmployee)
		{
			Employee employee = _employeeList.FirstOrDefault(x => x.Id == updatedEmployee.Id);
			if (employee != null)
			{
				employee.Name = updatedEmployee.Name;
				employee.Email = updatedEmployee.Email;
                employee.Department = updatedEmployee.Department;
                employee.PhotoPath = updatedEmployee.PhotoPath;
			}

            return employee;
        }
	}
}