using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
    }
}
