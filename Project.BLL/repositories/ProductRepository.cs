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
	public class ProductRepository:GenericRepository<Product> ,IProductRepository
	{
        public ProductRepository(AppDBContext context) : base(context)
        {
            
        }

        public async Task<IEnumerable<Product>> GetByName(string name)
        {
            return await _context.Products.Where(P=>P.Name.ToLower().Contains(name.ToLower())).Include(p=>p.Category).ToListAsync();

        }
    }
}
