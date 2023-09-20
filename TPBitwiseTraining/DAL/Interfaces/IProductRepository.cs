using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Interfaces
{
    public interface IProductRepository : IGenericRepository<Product>
    {
        public Task<IEnumerable<Product>> GetAllAsyncWithData();
        public Task<Product> GetByIdWithData(int id);
    }
}
