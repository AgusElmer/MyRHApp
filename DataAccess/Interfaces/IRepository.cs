using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyRHApp.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        List<T> GetAll();
        T GetById(int id);
        void Add(T entity);
        void Update(T entity);
        void Delete(int id);
    }

}
