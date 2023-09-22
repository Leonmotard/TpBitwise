using Microsoft.EntityFrameworkCore;
using TPBitwiseTraining.DAL.DataContext;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Implementations
{
    public class BrandRepository : GenericRepository<Brand>, IBrandRepository
    {
        private readonly ApplicationDbContext _context;

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Brand>> GetBrandWihtProducts()
        {
            return await _context.Brands.Include(p=> p.Products).ToListAsync(); 

        }
    }
}
