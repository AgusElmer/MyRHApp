using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private static List<Role> roles = new List<Role>();

        public List<Role> GetAll()
        {
            return roles;
        }

        public Role GetById(int id)
        {
            return roles.FirstOrDefault(r => r.Id == id);
        }

        public void Add(Role role)
        {
            roles.Add(role);
        }

        public void Update(Role role)
        {
            var existingRole = GetById(role.Id);
            if (existingRole != null)
            {
                existingRole.Name = role.Name;
            }
        }

        public void Delete(int id)
        {
            var role = GetById(id);
            if (role != null)
            {
                roles.Remove(role);
            }
        }
    }


}
