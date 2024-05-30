using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;

namespace MyRHApp.DataAccess.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static List<Employee> employees = new List<Employee>();

        public List<Employee> GetAll()
        {
            return employees;
        }

        public Employee GetById(int id)
        {
            return employees.FirstOrDefault(e => e.Id == id);
        }

        public void Add(Employee employee)
        {
            employees.Add(employee);
        }

        public void Update(Employee employee)
        {
            var existingEmployee = GetById(employee.Id);
            if (existingEmployee != null)
            {
                existingEmployee.Username = employee.Username;
                existingEmployee.Password = employee.Password;
                existingEmployee.Position = employee.Position;
                existingEmployee.HireDate = employee.HireDate;
                existingEmployee.Absences = employee.Absences;
                existingEmployee.ExtraHours = employee.ExtraHours;
                existingEmployee.VacationRequests = employee.VacationRequests;
            }
        }

        public void Delete(int id)
        {
            var employee = GetById(id);
            if (employee != null)
            {
                employees.Remove(employee);
            }
        }
    }

}
