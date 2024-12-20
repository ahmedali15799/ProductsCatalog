using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Project.DAL.Models;

namespace Project.BLL.Interfaces
{
	public interface IProductRepository : IGenericRepository<Product>
	{
		Task<IEnumerable<Product>> GetByName(string name);
	}
}
