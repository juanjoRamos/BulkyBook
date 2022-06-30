using BulkyBook.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BulkyBook.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<CategoryModel> CategoriesTable { get; set; }
        public DbSet<CoverTypeModel> CoverTypeTable { get; set; }
        public DbSet<ProductModel> ProductTable { get; set; }
        public DbSet<ApplicationUser> ApplicationUser { get; set; }
        public DbSet<Company> CompanyTable { get; set; }
        public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    }
}
