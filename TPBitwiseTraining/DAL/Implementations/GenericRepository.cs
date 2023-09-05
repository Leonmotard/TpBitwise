using TPBitwiseTraining.DAL.DataContext;
using TPBitwiseTraining.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TPBitwiseTraining.DAL.Implementations
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Update(T entity)
        {
            bool resultado = false;

            _context.Set<T>().Update(entity);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = await GetById(id);
            if (entidad == null)
            {
                return false;
            }
            _context.Set<T>().Remove(entidad);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> Insert(T entity)
        {
            bool resultado = false;
            _context.Set<T>().AddAsync(entity);
            resultado = await _context.SaveChangesAsync() > 0;
            return resultado;
        }

        public async Task<T> GetById(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await _context.Set<T>().ToListAsync();
        }
    }
}
