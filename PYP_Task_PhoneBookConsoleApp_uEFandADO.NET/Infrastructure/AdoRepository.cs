using Microsoft.Data.SqlClient;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Infrastructure
{
    public class AdoRepository<T> : IRepository<T> where T : class
    {
        SqlCommand _command;
        SqlConnection _connection;
        public AdoRepository()
        {
            _connection = new SqlConnection("Server=TABRIZ\\SQLEXPRESS;Database=PhoneBook;Trusted_Connection=True;");
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        public async Task<int> AddAync(T entity)
        {
            var sql = entity.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).Aggregate((x, y) => x + "," + y);
            var sql2 = entity.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => "@" + x.Name).Aggregate((x, y) => x + "," + y);
            var query = $"insert into {entity.GetType().Name+"s"} ({sql}) values ({sql2})";
            SqlCommand command = new SqlCommand(query, _connection);
            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name != "Id")
                {
                    command.Parameters.AddWithValue("@" + item.Name, item.GetValue(entity));
                }
            }
            await _connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }

        public async Task<int> DeleteAsync(T entity)
        {
            var query = $"delete from {entity.GetType().Name+"s"} where Id = {entity.GetType().GetProperty("Id").GetValue(entity)}";
            SqlCommand command = new SqlCommand(query, _connection);
            await _connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }


        public async Task<List<T>> GetAllAsync()
        {
            var query = $"select * from {typeof(T).Name+"s"}";
            var command = new SqlCommand(query, _connection);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            var list = new List<T>();
            while (reader.Read())
            {
                var obj = Activator.CreateInstance<T>();
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var property = obj.GetType().GetProperty(reader.GetName(i));
                    property.SetValue(obj, reader.GetValue(i));
                }
                list.Add(obj);
            }
            await _connection.CloseAsync();
            return list;
        }


        public async Task<T> GetByIdAsync(int id)
        {
            var query = $"select * from {typeof(T).Name+"s"} where Id={id}";
            var command = new SqlCommand(query, _connection);
            await _connection.OpenAsync();
            var reader = await command.ExecuteReaderAsync();
            var obj = Activator.CreateInstance<T>();
            while (reader.Read())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    var property = obj.GetType().GetProperty(reader.GetName(i));
                    property.SetValue(obj, reader.GetValue(i));
                }
            }
            await _connection.CloseAsync();
            return obj;
        }


        public async Task<int> UpdateAsync(T entity)
        {

            var sql = entity.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name + " = @" + x.Name).Aggregate((x, y) => x + "," + y);
            var query = $"update {entity.GetType().Name+"s"} set {sql} where Id = {entity.GetType().GetProperty("Id").GetValue(entity)}";
            SqlCommand command = new SqlCommand(query, _connection);
            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name != "Id")
                {
                    command.Parameters.AddWithValue("@" + item.Name, item.GetValue(entity));
                }
            }
            await _connection.OpenAsync();
            var result = await command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }
    }
}
