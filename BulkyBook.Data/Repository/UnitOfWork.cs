using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public ICategoryRepository categoryRepository { get; private set; }
        public ICoverTypeRepository coverTypeRepository { get; private set; }
        public IProductRepository productRepository { get; private set; }
        public ICompanyRepository companyRepository { get; private set; }
        public IApplicationUserRepository applicationUserRepository { get; private set; }
        public IShoppingCartRepository shoppingCartRepository { get; private set; }

        public ApplicationDbContext _db;
        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            categoryRepository = new CategoryRepository(_db);
            coverTypeRepository = new CoverTypeRepository(_db);
            productRepository = new ProductRepository(_db);
            companyRepository = new CompanyRepository(_db);
            applicationUserRepository = new ApplicationUserRepository(_db);
            shoppingCartRepository = new ShoppingCartRepository(_db);
        }

        void IUnitOfWork.Save()
        {
            _db.SaveChanges();
        }

    }
}
