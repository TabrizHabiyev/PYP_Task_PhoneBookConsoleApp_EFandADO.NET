using Microsoft.EntityFrameworkCore;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.DataContext;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Infrastructure
{
    // Ef repository

    public class EfRepository<T> : IRepository<T> where T : class
    {
        private readonly PhoneBookContext _context;
        private readonly DbSet<T> _dbSet;
        public EfRepository(PhoneBookContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }
        public async Task<int> AddAync(T entity)
        {
            await _dbSet.AddAsync(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<int> DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
            return await _context.SaveChangesAsync();
        }
        public async Task<List<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<T> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }
        public async Task<int> UpdateAsync(T entity)
        {
            _dbSet.Update(entity);
            return await _context.SaveChangesAsync();
        }
    }
}
