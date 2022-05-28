using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class ProductRepository : Repository<ProductModel>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        
        public ProductRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(ProductModel obj)
        {
            var objToDb = _db.ProductTable.FirstOrDefault(product => product.Id == obj.Id);
            if (objToDb != null)
            {
                objToDb.Title = obj.Title;
                objToDb.ISBN = obj.ISBN;
                objToDb.Price = obj.Price;
                objToDb.Description = obj.Description;
                objToDb.Author = obj.Author;
                objToDb.CategoryId = obj.CategoryId;
                objToDb.CoverTypeId = obj.CoverTypeId;
                if (objToDb.ImageUrl != null) 
                {
                    objToDb.ImageUrl = obj.ImageUrl;
                }
            }
        }
    }
}
