using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Project.BLL.Interfaces;
using Project.DAL.Data;
using Project.DAL.Models;

namespace Project.BLL.repositories
{
    public class GenericRepository<T> :IGenericRepository<T> where T : BaseEntity
    {
        private protected AppDBContext _context;


        public GenericRepository(AppDBContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
                _context.Add(entity);
        }

        public void Delete(T entity)
        {
                _context.Remove(entity);
        }

        public T Get(int id)
        {
            //var product = _context.Products.FirstOrDefault(D => D.Id == id);
            var result = _context.Set<T>().Find(id);
            return result;
        }

        public IEnumerable<T> GetAll()
        {
            //var Products = _context.Products.ToList();
            if (typeof(T) == typeof(Product))
            {
                return (IEnumerable<T>)_context.Products.Include(E => E.Category).ToList();
            }

            else
            {
                var result = _context.Set<T>().ToList();
                return result;
            }
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }
    }
}
