namespace IbrahimPortfolio.DataAccess.Abstract
{
    public interface IGenericRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();

        Task<T?> GetByIdAsync(int id);

        Task CreateAsync(T entity);

        void Update(T entity);

        void Delete(T entity);

        Task<int> SaveChangesAsync();
    }
}