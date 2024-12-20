using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.BLL.Interfaces;
using Project.BLL.repositories;
using Project.DAL.Data;

namespace Project.BLL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDBContext _context;
		private Lazy<IProductRepository> productRepository;
        private Lazy<ICategoryRepository> categoryRepository;


        public UnitOfWork(AppDBContext context)
        {
            _context = context;
			productRepository= new Lazy<IProductRepository>(new ProductRepository(_context));
            categoryRepository= new Lazy<ICategoryRepository>(new CategoryRepository(_context));
        }
		public IProductRepository ProductRepository => productRepository.Value;
        public ICategoryRepository CategoryRepository => categoryRepository.Value;


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
