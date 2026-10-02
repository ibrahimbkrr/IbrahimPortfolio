using IbrahimPortfolio.DataAccess.Abstract;
using IbrahimPortfolio.DataAccess.Context;
using Microsoft.EntityFrameworkCore;

namespace IbrahimPortfolio.DataAccess.Concrete
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            _context = context;
        }

        private DbSet<T> Table => _context.Set<T>();

        public async Task<List<T>> GetAllAsync()
        {
            return await Table.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await Table.FindAsync(id);
        }

        public async Task CreateAsync(T entity)
        {
            await Table.AddAsync(entity);
        }

        public void Update(T entity)
        {
            Table.Update(entity);
        }

        public void Delete(T entity)
        {
            Table.Remove(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}