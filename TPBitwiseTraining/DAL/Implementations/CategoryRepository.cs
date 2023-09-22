using Microsoft.EntityFrameworkCore;
using TPBitwiseTraining.DAL.DataContext;
using TPBitwiseTraining.DAL.Interfaces;
using TPBitwiseTraining.Models;

namespace TPBitwiseTraining.DAL.Implementations
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Category> GetByIdWithProducts(int id)
        {
            var query = await _context.Categories.Include(p => p.Products)
                .FirstOrDefaultAsync(c => c.Id == id);
            
            return query;
        }
    }
}
