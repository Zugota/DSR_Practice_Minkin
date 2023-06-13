using DSRPractice.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DSRPractice.Services
{
    public class SQLEmployeeRepository : IEmployeeRepository
    {
        private readonly AppDBContext _context;

        public SQLEmployeeRepository(AppDBContext context)
        {
            _context = context;
        }

        public Employee Add(Employee newEmployee)
        {
            _context.Employees.Add(newEmployee);
            _context.SaveChanges();
            return newEmployee;
        }

        public Employee Delete(int id)
        {
            var employeeToDel = _context.Employees.FirstOrDefault(x => x.Id == id);
            if (employeeToDel != null)
            {
                _context.Employees.Remove(employeeToDel);
                _context.SaveChanges();
            }
            return employeeToDel;
        }

        public IEnumerable<DeptHeadCount> empCountInDept(Dept? dept)
        {
            IEnumerable<Employee> query = _context.Employees;

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
            return _context.Employees;
        }

        public Employee GetEmployee(int id)
        {
            /*return _context.Employees.FirstOrDefault(x => x.Id == id);*/
            return _context.Employees.FromSqlRaw<Employee>("CodeSPGetEmpByID {0}", id).ToList().FirstOrDefault();
        }

        public IEnumerable<Employee> Search(string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return _context.Employees;
            }
            else
            {
                return _context.Employees.Where(x => x.Name.ToLower().Contains(term.ToLower()) || x.Email.Contains(term.ToLower()));
            }
        }

        public Employee Update(Employee updatedEmployee)
        {
            var employee = _context.Employees.Attach(updatedEmployee);
            employee.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            _context.SaveChanges();
            return updatedEmployee;
        }
    }
}
