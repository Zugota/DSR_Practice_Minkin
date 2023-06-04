using DSRPractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPractice.Services
{
	public interface IEmployeeRepository
	{
		IEnumerable<Employee> GetAllEmployees();

		Employee GetEmployee(int id);

		Employee Update(Employee updatedEmployee);
	}
}
