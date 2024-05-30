using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.Services.Interfaces
{
    public interface IRoleService
    {
        void CreateRole(Role role);
        Role GetRoleById(int id);
        List<Role> GetAllRoles();
    }
}
