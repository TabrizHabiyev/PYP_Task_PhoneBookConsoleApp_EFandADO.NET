using Microsoft.Data.SqlClient;
using System.Collections;
using System.Linq.Expressions;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;

public interface IRepository<T> where T : class
{
    Task<int> AddAync(T entity);
    Task<int> UpdateAsync(T entity);
    Task<int> DeleteAsync(T entity);
    Task<T> GetByIdAsync(int id);
    IEnumerable<T> Search(Expression<Func<T, bool>> func);
}
