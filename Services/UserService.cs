using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using MyRHApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyRHApp.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserRoleContextRepository _userRoleContextRepository;
        private readonly IRoleService _roleService;

        public UserService(IUserRepository userRepository, IUserRoleContextRepository userRoleContextRepository, IRoleService roleService)
        {
            _userRepository = userRepository;
            _userRoleContextRepository = userRoleContextRepository;
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

        public void Register(User user)
        {
            _userRepository.Add(user);
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
            Console.WriteLine($"Assigned RoleId {roleId} to UserId {userId} in ContextId {contextId}");
        }

        public List<Role> GetRolesForUserInContext(int userId, int contextId)
        {
            var userRoleContexts = _userRoleContextRepository.GetByUserId(userId)
                                                              .Where(urc => urc.ContextId == contextId)
                                                              .ToList();
            Console.WriteLine($"UserRoleContexts count for userId {userId} and contextId {contextId}: {userRoleContexts.Count}");

            var roles = new List<Role>();
            foreach (var urc in userRoleContexts)
            {
                var role = _roleService.GetRoleById(urc.RoleId);
                if (role != null)
                {
                    roles.Add(role);
                }
            }
            Console.WriteLine($"Roles count for userId {userId} in ContextId {contextId}: {roles.Count}");
            return roles;
        }
    }
}
