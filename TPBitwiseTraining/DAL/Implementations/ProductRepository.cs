using Microsoft.EntityFrameworkCore;
using TPBitwiseTraining.DAL.DataContext;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Implementations
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Product>> GetAllAsyncWithData()
        {
            return await _context.Products.Include(b => b.Brand).Include(c => c.Category).ToListAsync();
        }

        public async Task<Product> GetByIdWithData(int id)
        {
            var query = await _context.Products
                                 .Include(b => b.Brand)
                                 .Include(c => c.Category)
                                 .FirstOrDefaultAsync(p => p.Id == id);
            
            return query;
        }
    }
    
}
