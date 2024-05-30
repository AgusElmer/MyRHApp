using MyRHApp.Models;
using System.Collections.Generic;

namespace MyRHApp.Services.Interfaces
{
    public interface IEmployeeService
    {
        void CreateEmployee(Employee employee);
        Employee GetEmployeeById(int id);
        List<Employee> GetEmployees();
        void UpdateEmployee(Employee employee);
        void DeleteEmployee(int id);
    }
}
