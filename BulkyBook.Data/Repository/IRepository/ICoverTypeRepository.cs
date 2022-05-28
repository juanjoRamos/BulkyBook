using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface ICoverTypeRepository: IRepository<CoverTypeModel>
    {
        void Update(CoverTypeModel obj);
    }
}
