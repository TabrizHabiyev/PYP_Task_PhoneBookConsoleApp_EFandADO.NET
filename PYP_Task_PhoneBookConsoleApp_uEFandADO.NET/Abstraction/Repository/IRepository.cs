namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;

public interface IRepository<T>
{
    Task<int> AddAync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<List<T>> GetAllAsync();
}
