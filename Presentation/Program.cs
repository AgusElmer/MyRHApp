using System;
using System.Linq;
using MyRHApp.Models;
using MyRHApp.Services;
using MyRHApp.Services.Interfaces;
using MyRHApp.Utilities;
using MyRHApp.DataAccess.Interfaces;
using MyRHApp.DataAccess.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MyRHApp
{
    class Program
    {
        private static IUserService _userService;
        private static IEmployeeService _employeeService;
        private static IRoleService _roleService;
        private static User _currentUser;
        private static Context _currentContext;

        static void Main(string[] args)
        {
            ConfigureServices();
            SeedData();
            Login();

            while (_currentUser != null)
            {
                Console.Clear();
                Console.WriteLine("Human Resources Console Application");
                SetContext();
                var roles = _userService.GetRolesForUserInContext(_currentUser.Id, _currentContext.Id);
                if (roles.Any(r => r.Name == "HR"))
                {
                    ShowHROptions();
                }
                else if (roles.Any(r => r.Name == "Employee"))
                {
                    ShowEmployeeOptions();
                }
            }
        }

        private static void ConfigureServices()
        {
            var serviceProvider = new ServiceCollection()
            .AddSingleton<IUserRepository, UserRepository>()
            .AddSingleton<IUserRoleContextRepository, UserRoleContextRepository>()
            .AddSingleton<IRoleRepository, RoleRepository>()
            .AddSingleton<IEmployeeRepository, EmployeeRepository>()
            .AddSingleton<IContextRepository, ContextRepository>()
            .AddSingleton<IUserService, UserService>()
            .AddSingleton<IRoleService, RoleService>()
            .AddSingleton<IEmployeeService, EmployeeService>()
            .BuildServiceProvider();

            _userService = serviceProvider.GetService<IUserService>();
            _employeeService = serviceProvider.GetService<IEmployeeService>();
            _roleService = serviceProvider.GetService<IRoleService>();
        }

        private static void SeedData()
        {
            // Create roles
            var hrRole = new Role { Id = 1, Name = "HR" };
            var employeeRole = new Role { Id = 2, Name = "Employee" };

            // Add roles to repository through service
            _roleService.CreateRole(hrRole);
            _roleService.CreateRole(employeeRole);

            // Create employees
            var hrEmployee = new HREmployee { Id = 1, FirstName = "HR", LastName = "User", Position = "HR Manager", HireDate = DateTime.Now };
            var generalEmployee = new GeneralEmployee { Id = 2, FirstName = "Employee", LastName = "User", Position = "Developer", HireDate = DateTime.Now };

            // Create users with EmployeeId
            var hrUser = new User { Id = 1, Username = "hruser", Password = "password", EmployeeId = hrEmployee.Id };
            var generalUser = new User { Id = 2, Username = "empuser", Password = "password", EmployeeId = generalEmployee.Id };

            _userService.Register(hrUser, hrEmployee);
            _userService.Register(generalUser, generalEmployee);

            // Assign roles to users in context 1
            _userService.AssignRoleToUser(hrUser.Id, hrRole.Id, 1);
            _userService.AssignRoleToUser(generalUser.Id, employeeRole.Id, 1);

            Console.WriteLine("Seed data created successfully.");
        }

        private static void Login()
        {
            while (_currentUser == null)
            {
                Console.Clear();
                Console.WriteLine("Login to Human Resources Console Application");
                Console.Write("Username: ");
                var username = Console.ReadLine();
                Console.Write("Password: ");
                var password = Console.ReadLine();

                _currentUser = _userService.Authenticate(username, password);
                if (_currentUser == null)
                {
                    Console.WriteLine("Invalid username or password. Please try again.");
                    Console.ReadKey();
                }
            }
        }

        private static void SetContext()
        {
            Console.WriteLine("Select Context:");
            Console.WriteLine("1. Default Context");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();
            _currentContext = new Context { Id = 1, Name = "Default Context" }; // Context selection for example
        }

        private static void ShowHROptions()
        {
            Console.WriteLine("1. List Employees");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Logout");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ListEmployees();
                    break;
                case "2":
                    AddEmployee();
                    break;
                case "3":
                    UpdateEmployee();
                    break;
                case "4":
                    DeleteEmployee();
                    break;
                case "5":
                    _currentUser = null;
                    Login();
                    break;
            }
        }

        private static void ShowEmployeeOptions()
        {
            Console.WriteLine("1. View Profile");
            Console.WriteLine("2. Submit Absence");
            Console.WriteLine("3. Submit Extra Hours");
            Console.WriteLine("4. Request Vacation");
            Console.WriteLine("5. Logout");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    ViewProfile();
                    break;
                case "2":
                    SubmitAbsence();
                    break;
                case "3":
                    SubmitExtraHours();
                    break;
                case "4":
                    RequestVacation();
                    break;
                case "5":
                    _currentUser = null;
                    Login();
                    break;
            }
        }

        private static void ListEmployees()
        {
            Console.Clear();
            var employees = _employeeService.GetEmployees();
            foreach (var emp in employees)
            {
                Console.WriteLine($"{emp.Id}: {emp.FirstName} {emp.LastName}, {emp.Position}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("Select Employee Type:");
            Console.WriteLine("1. General Employee");
            Console.WriteLine("2. HR Employee");
            Console.Write("Select an option: ");
            var option = Console.ReadLine();

            Employee employee;

            if (option == "1")
            {
                employee = new GeneralEmployee();
            }
            else if (option == "2")
            {
                employee = new HREmployee();
            }
            else
            {
                Console.WriteLine("Invalid option selected.");
                Console.ReadKey();
                return;
            }

            Console.Write("First Name: ");
            employee.FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            employee.LastName = Console.ReadLine();
            Console.Write("Position: ");
            employee.Position = Console.ReadLine();
            Console.Write("Hire Date (yyyy-mm-dd): ");
            employee.HireDate = DateTime.Parse(Console.ReadLine());

            // Create employee
            _employeeService.CreateEmployee(employee);

            // Create user
            var user = new User
            {
                Username = $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}",
                Password = "password", // Default password
                EmployeeId = employee.Id
            };

            _userService.Register(user, employee);

            Console.WriteLine("Employee and user added successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void UpdateEmployee()
        {
            Console.Clear();
            Console.Write("Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            Console.Write("First Name: ");
            employee.FirstName = Console.ReadLine();
            Console.Write("Last Name: ");
            employee.LastName = Console.ReadLine();
            Console.Write("Position: ");
            employee.Position = Console.ReadLine();
            Console.Write("Hire Date (yyyy-mm-dd): ");
            employee.HireDate = DateTime.Parse(Console.ReadLine());
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Employee updated successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void DeleteEmployee()
        {
            Console.Clear();
            Console.Write("Employee ID: ");
            int id = int.Parse(Console.ReadLine());
            _employeeService.DeleteEmployee(id);
            Console.WriteLine("Employee deleted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void ViewProfile()
        {
            Console.Clear();
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            if (employee != null)
            {
                Console.WriteLine($"Name: {employee.FirstName} {employee.LastName}");
                Console.WriteLine($"Position: {employee.Position}");
                Console.WriteLine($"Hire Date: {employee.HireDate}");
            }
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void SubmitAbsence()
        {
            Console.Clear();
            var absence = new Absence();
            Console.Write("Date (yyyy-mm-dd): ");
            absence.Date = DateTime.Parse(Console.ReadLine());
            Console.Write("Reason: ");
            absence.Reason = Console.ReadLine();
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.Absences.Add(absence);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Absence submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void SubmitExtraHours()
        {
            Console.Clear();
            var extraHour = new ExtraHour();
            Console.Write("Date (yyyy-mm-dd): ");
            extraHour.Date = DateTime.Parse(Console.ReadLine());
            Console.Write("Hours: ");
            extraHour.Hours = int.Parse(Console.ReadLine());
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.ExtraHours.Add(extraHour);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Extra hours submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private static void RequestVacation()
        {
            Console.Clear();
            var vacationRequest = new VacationRequest();
            Console.Write("Start Date (yyyy-mm-dd): ");
            vacationRequest.StartDate = DateTime.Parse(Console.ReadLine());
            Console.Write("End Date (yyyy-mm-dd): ");
            vacationRequest.EndDate = DateTime.Parse(Console.ReadLine());
            vacationRequest.IsApproved = false;
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.VacationRequests.Add(vacationRequest);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Vacation request submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }
    }
}
