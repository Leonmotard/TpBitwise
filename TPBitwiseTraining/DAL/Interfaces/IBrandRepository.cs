using TPBitwiseTraining.DAL.Implementations;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Interfaces
{
    public interface IBrandRepository : IGenericRepository<Brand>
    {
        public Task<IEnumerable<Brand>> GetBrandWihtProducts();
    }
}
