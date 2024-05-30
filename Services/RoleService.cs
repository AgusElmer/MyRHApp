using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using MyRHApp.Services.Interfaces;
using System.Collections.Generic;

namespace MyRHApp.Services
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        public void CreateRole(Role role)
        {
            _roleRepository.Add(role);
        }

        public Role GetRoleById(int id)
        {
            return _roleRepository.GetById(id);
        }

        public List<Role> GetAllRoles()
        {
            return _roleRepository.GetAll();
        }
    }
}
