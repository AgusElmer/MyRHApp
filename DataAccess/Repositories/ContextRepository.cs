using MyRHApp.DataAccess.Interfaces;
using MyRHApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Repositories
{
    public class ContextRepository : IContextRepository
    {
        private static List<Context> contexts = new List<Context>();

        public List<Context> GetAll()
        {
            return contexts;
        }

        public Context GetById(int id)
        {
            return contexts.FirstOrDefault(c => c.Id == id);
        }

        public void Add(Context context)
        {
            contexts.Add(context);
        }

        public void Update(Context entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }
    }

}
