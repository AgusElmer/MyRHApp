using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using MyRHApp.Services.Interfaces;
using System.Collections.Generic;

namespace MyRHApp.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public void CreateEmployee(Employee employee)
        {
            _employeeRepository.Add(employee);
        }

        public Employee GetEmployeeById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        public List<Employee> GetEmployees()
        {
            return _employeeRepository.GetAll();
        }

        public void UpdateEmployee(Employee employee)
        {
            _employeeRepository.Update(employee);
        }

        public void DeleteEmployee(int id)
        {
            _employeeRepository.Delete(id);
        }
    }
}
