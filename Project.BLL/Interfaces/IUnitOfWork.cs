using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.BLL.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
		public IProductRepository ProductRepository { get;}
        public ICategoryRepository CategoryRepository { get;}


        int Complete();


    }
}
