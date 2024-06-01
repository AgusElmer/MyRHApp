using System;
using System.Linq;
using MyRHApp.Models;
using MyRHApp.Services.Interfaces;
using MyRHApp.Utilities;

namespace MyRHApp
{
    public class ConsoleManager
    {
        private IUserService _userService;
        private IEmployeeService _employeeService;
        private IRoleService _roleService;
        private User _currentUser;
        private Context _currentContext;

        public ConsoleManager(IUserService userService, IEmployeeService employeeService, IRoleService roleService)
        {
            _userService = userService;
            _employeeService = employeeService;
            _roleService = roleService;
        }

        public void Start()
        {
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

        private void Login()
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

        private void SetContext()
        {
            _currentContext = new Context { Id = 1, Name = "Default Context" }; // Context selection for example
        }

        private void ShowHROptions()
        {
            Console.WriteLine("1. List Employees");
            Console.WriteLine("2. Add Employee");
            Console.WriteLine("3. Update Employee");
            Console.WriteLine("4. Delete Employee");
            Console.WriteLine("5. Logout");
            int option = InputValidator.GetValidatedInt("Select an option: ");

            switch (option)
            {
                case 1:
                    ListEmployees();
                    break;
                case 2:
                    AddEmployee();
                    break;
                case 3:
                    UpdateEmployee();
                    break;
                case 4:
                    DeleteEmployee();
                    break;
                case 5:
                    _currentUser = null;
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }

        private void ShowEmployeeOptions()
        {
            Console.WriteLine("1. View Profile");
            Console.WriteLine("2. Submit Absence");
            Console.WriteLine("3. Submit Extra Hours");
            Console.WriteLine("4. Request Vacation");
            Console.WriteLine("5. Logout");
            int option = InputValidator.GetValidatedInt("Select an option: ");

            switch (option)
            {
                case 1:
                    ViewProfile();
                    break;
                case 2:
                    SubmitAbsence();
                    break;
                case 3:
                    SubmitExtraHours();
                    break;
                case 4:
                    RequestVacation();
                    break;
                case 5:
                    _currentUser = null;
                    Login();
                    break;
                default:
                    Console.WriteLine("Invalid option selected.");
                    break;
            }
        }

        private void ListEmployees()
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

        private void AddEmployee()
        {
            Console.Clear();
            Console.WriteLine("Select Employee Type:");
            Console.WriteLine("1. General Employee");
            Console.WriteLine("2. HR Employee");
            int option = InputValidator.GetValidatedInt("Select an option: ");

            Employee employee;
            int userRole;

            if (option == 1)
            {
                employee = new Employee();
                userRole = (int)UserRole.Employee;
            }
            else if (option == 2)
            {
                employee = new Employee();
                userRole = (int)UserRole.HR;
            }
            else
            {
                Console.WriteLine("Invalid option selected.");
                Console.ReadKey();
                return;
            }

            employee.FirstName = InputValidator.GetNonEmptyString("First Name: ");
            employee.LastName = InputValidator.GetNonEmptyString("Last Name: ");
            employee.Position = InputValidator.GetNonEmptyString("Position: ");
            employee.HireDate = InputValidator.GetValidatedDate("Hire Date (yyyy-mm-dd): ");

            var user = new User
            {
                Username = $"{employee.FirstName.ToLower()}.{employee.LastName.ToLower()}",
                Password = "password", // Default password
                EmployeeId = employee.Id
            };

            user = _userService.Register(user, employee);
            _userService.AssignRoleToUser(user.Id, userRole, 1);

            Console.WriteLine("Employee and user added successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void UpdateEmployee()
        {
            Console.Clear();
            int id = InputValidator.GetValidatedInt("Employee ID: ");
            var employee = _employeeService.GetEmployeeById(id);
            if (employee == null)
            {
                Console.WriteLine("Employee not found!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                return;
            }
            employee.FirstName = InputValidator.GetNonEmptyString("First Name: ");
            employee.LastName = InputValidator.GetNonEmptyString("Last Name: ");
            employee.Position = InputValidator.GetNonEmptyString("Position: ");
            employee.HireDate = InputValidator.GetValidatedDate("Hire Date (yyyy-mm-dd): ");
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Employee updated successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void DeleteEmployee()
        {
            Console.Clear();
            int id = InputValidator.GetValidatedInt("Employee ID: ");
            _employeeService.DeleteEmployee(id);
            Console.WriteLine("Employee deleted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void ViewProfile()
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

        private void SubmitAbsence()
        {
            Console.Clear();
            var absence = new Absence
            {
                Date = InputValidator.GetValidatedDate("Date (yyyy-mm-dd): "),
                Reason = InputValidator.GetNonEmptyString("Reason: ")
            };
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.Absences.Add(absence);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Absence submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void SubmitExtraHours()
        {
            Console.Clear();
            var extraHour = new ExtraHour
            {
                Date = InputValidator.GetValidatedDate("Date (yyyy-mm-dd): "),
                Hours = InputValidator.GetValidatedInt("Hours: ")
            };
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.ExtraHours.Add(extraHour);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Extra hours submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void RequestVacation()
        {
            Console.Clear();
            var vacationRequest = new VacationRequest
            {
                StartDate = InputValidator.GetValidatedDate("Start Date (yyyy-mm-dd): "),
                EndDate = InputValidator.GetValidatedDate("End Date (yyyy-mm-dd): "),
                IsApproved = false
            };
            var employee = _employeeService.GetEmployeeById(_currentUser.Id);
            employee.VacationRequests.Add(vacationRequest);
            _employeeService.UpdateEmployee(employee);
            Console.WriteLine("Vacation request submitted successfully!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey();
        }

        private void SeedData()
        {
            var hrRole = new Role { Id = (int)UserRole.HR, Name = "HR" };
            var employeeRole = new Role { Id = (int)UserRole.Employee, Name = "Employee" };

            _roleService.CreateRole(hrRole);
            _roleService.CreateRole(employeeRole);

            var hrEmployee = new Employee { Id = 1, FirstName = "HR", LastName = "User", Position = "HR Manager", HireDate = DateTime.Now };
            var generalEmployee = new Employee { Id = 2, FirstName = "Employee", LastName = "User", Position = "Developer", HireDate = DateTime.Now };

            var hrUser = new User { Id = 1, Username = "hruser", Password = "password", EmployeeId = hrEmployee.Id };
            var generalUser = new User { Id = 2, Username = "empuser", Password = "password", EmployeeId = generalEmployee.Id };

            _userService.Register(hrUser, hrEmployee);
            _userService.Register(generalUser, generalEmployee);

            _userService.AssignRoleToUser(hrUser.Id, hrRole.Id, 1);
            _userService.AssignRoleToUser(generalUser.Id, employeeRole.Id, 1);

            Console.WriteLine("Seed data created successfully.");
        }
    }
}
