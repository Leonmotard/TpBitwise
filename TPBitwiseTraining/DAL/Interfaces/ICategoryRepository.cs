using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public Task<Category> GetByIdWithProducts(int id);
        
    }
}
