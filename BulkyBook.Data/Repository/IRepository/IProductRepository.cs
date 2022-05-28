using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface IProductRepository : IRepository<ProductModel>
    {
        void Update(ProductModel obj);
    }
}
