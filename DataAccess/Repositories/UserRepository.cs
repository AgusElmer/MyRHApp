using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private static List<User> users = new List<User>();

        public List<User> GetAll()
        {
            return users;
        }

        public User GetById(int id)
        {
            return users.FirstOrDefault(u => u.Id == id);
        }

        public User GetByUsername(string username)
        {
            return users.FirstOrDefault(u => u.Username == username);
        }

        public void Add(User user)
        {
            users.Add(user);
        }

        public void Update(User user)
        {
            var existingUser = GetById(user.Id);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
            }
        }

        public void Delete(int id)
        {
            var user = GetById(id);
            if (user != null)
            {
                users.Remove(user);
            }
        }
    }



}
