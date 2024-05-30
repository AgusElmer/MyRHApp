using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Repositories
{
    public class UserRoleContextRepository : IUserRoleContextRepository
    {
        private static List<UserRoleContext> userRoleContexts = new List<UserRoleContext>();

        public List<UserRoleContext> GetAll()
        {
            return userRoleContexts;
        }

        public UserRoleContext GetById(int id)
        {
            return userRoleContexts.FirstOrDefault(urc => urc.Id == id);
        }

        public List<UserRoleContext> GetByUserId(int userId)
        {
            return userRoleContexts.Where(urc => urc.Id == userId).ToList();
        }

        public void Add(UserRoleContext userRoleContext)
        {
            userRoleContexts.Add(userRoleContext);
        }

        public void Update(UserRoleContext userRoleContext)
        {
            var existingUserRoleContext = GetById(userRoleContext.Id);
            if (existingUserRoleContext != null)
            {
                existingUserRoleContext.RoleId = userRoleContext.RoleId;
                existingUserRoleContext.ContextId = userRoleContext.ContextId;
            }
        }

        public void Delete(int id)
        {
            var userRoleContext = GetById(id);
            if (userRoleContext != null)
            {
                userRoleContexts.Remove(userRoleContext);
            }
        }
    }


}
