using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using MyRHApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRHApp.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleContextRepository _userRoleContextRepository;
        private readonly IEmployeeService _employeeService;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IUserRoleContextRepository userRoleContextRepository, IEmployeeService employeeService, IRoleService roleService)
        {
            _userRepository = userRepository;
            _userRoleContextRepository = userRoleContextRepository;
            _employeeService = employeeService;
            _roleService = roleService;
        }

        public User Authenticate(string username, string password)
        {
            var user = _userRepository.GetByUsername(username);
            if (user != null && user.Password == password)
            {
                return user;
            }
            return null;
        }

        public User Register(User user, Employee employee)
        {
            int id = 0;
            if (employee.Id == 0)
            {
                List<User> list = _userRepository.GetAll();
                if (list.Count() == 0)
                {
                    id = 1;
                }
                else
                {
                    id = list.Max(x => x.Id);
                    employee.Id = id + 1;
                }
            }
            _employeeService.CreateEmployee(employee);
            user.EmployeeId = employee.Id;
            user.Id = employee.Id;
            _userRepository.Add(user);
            return user;
        }

        public void AssignRoleToUser(int userId, int roleId, int contextId)
        {
            var userRoleContext = new UserRoleContext
            {
                Id = userId,
                RoleId = roleId,
                ContextId = contextId
            };
            _userRoleContextRepository.Add(userRoleContext);
        }

        public List<Role> GetRolesForUserInContext(int userId, int contextId)
        {
            var userRoleContexts = _userRoleContextRepository.GetByUserId(userId)
                                                              .Where(urc => urc.ContextId == contextId)
                                                              .ToList();

            var roles = new List<Role>();
            foreach (var urc in userRoleContexts)
            {
                var role = _roleService.GetRoleById(urc.RoleId);
                if (role != null)
                {
                    roles.Add(role);
                }
            }
            return roles;
        }
    }

}
