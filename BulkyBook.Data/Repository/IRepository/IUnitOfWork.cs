using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IUnitOfWork
    {
        public ICategoryRepository categoryRepository { get; }
        public ICoverTypeRepository coverTypeRepository { get; }
        public IProductRepository productRepository { get; }
        public ICompanyRepository companyRepository { get; }

        void Save();
    }
}
