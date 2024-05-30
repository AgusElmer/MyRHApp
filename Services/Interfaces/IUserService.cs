using MyRHApp.Models;
using System.Collections.Generic;

namespace MyRHApp.Services.Interfaces
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        void Register(User user, Employee employee);
        void AssignRoleToUser(int userId, int roleId, int contextId);
        List<Role> GetRolesForUserInContext(int userId, int contextId);
    }
}
