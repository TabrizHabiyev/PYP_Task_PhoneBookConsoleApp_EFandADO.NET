using Microsoft.Data.SqlClient;
using Pluralize.NET.Core;
using PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Repository;
using System.Linq.Expressions;

namespace PYP_Task_PhoneBookConsoleApp_EFandADO.NET.Infrastructure
{
    public class AdoRepository<T> : IRepository<T> where T : class
    {
        SqlCommand _command;
        SqlConnection _connection;
        public AdoRepository(SqlConnection connection)
        {
            _connection = connection;
            _command = new SqlCommand();
            _command.Connection = _connection;
        }

        private readonly string tableName = new Pluralizer().Pluralize(typeof(T).Name);

        public async Task<int> AddAync(T entity)
        {
            var sql = entity.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => x.Name).Aggregate((x, y) => x + "," + y);
            var sql2 = entity.GetType().GetProperties().Where(x => x.Name != "Id").Select(x => "@" + x.Name).Aggregate((x, y) => x + "," + y);
            var query = $"insert into {tableName} ({sql}) values ({sql2})";
            _command.CommandText = query;
            foreach (var item in entity.GetType().GetProperties()) 
            {
                if (item.Name != "Id")
                {
                    _command.Parameters.AddWithValue("@" + item.Name, item.GetValue(entity));
                } 
            }
            await _connection.OpenAsync();
            var result = await _command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }



        public async Task<int> DeleteAsync(T entity)
        {
            var query = $"delete from {tableName} where Id = {entity.GetType().GetProperty("Id").GetValue(entity)}";
            _command.CommandText = query;
            await _connection.OpenAsync(); 
            var result = await _command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }


        public async Task<List<T>> GetAllAsync()
        {
            var query = $"select * from {tableName}";
            _command.CommandText = query;
            await _connection.OpenAsync();
            var reader = await _command.ExecuteReaderAsync();
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
            var query = $"select * from {tableName} where Id={id}";
            _command.CommandText = query;
            await _connection.OpenAsync();
            var reader = await _command.ExecuteReaderAsync();
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
            var query = $"update {tableName} set {sql} where Id = {entity.GetType().GetProperty("Id").GetValue(entity)}";
            _command.CommandText = query;
            foreach (var item in entity.GetType().GetProperties())
            {
                if (item.Name != "Id")
                {
                    _command.Parameters.AddWithValue("@" + item.Name, item.GetValue(entity));
                }
            }
            await _connection.OpenAsync();
            var result = await _command.ExecuteNonQueryAsync();
            await _connection.CloseAsync();
            return result;
        }



        public IEnumerable<T> Search(Expression<Func<T, bool>> func)
        {
            string paramName = string.Empty;
            string searchValue = string.Empty;
            string query = String.Empty;

            if (func.Body is BinaryExpression)
            {
                var binaryExpression = func.Body as BinaryExpression;
                if (binaryExpression.NodeType == ExpressionType.Equal)
                {
                    paramName = (binaryExpression.Left as MemberExpression).Member.Name;
                    searchValue = (binaryExpression.Right as ConstantExpression).Value.ToString();
                    query = $"select * from {tableName} where {paramName} = '{searchValue}'";
                }
                else if (binaryExpression.NodeType == ExpressionType.AndAlso)
                {
                    var left = binaryExpression.Left as BinaryExpression;
                    var right = binaryExpression.Right as BinaryExpression;
                    string leftParamName = (left.Left as MemberExpression).Member.Name;
                    string leftSearchValue = (left.Right as ConstantExpression).Value.ToString();
                    string rightParamName = (right.Left as MemberExpression).Member.Name;
                    string rightSearchValue = (right.Right as ConstantExpression).Value.ToString();
                    query = $"select * from {tableName} where {leftParamName} = '{leftSearchValue}' and {rightParamName} = '{rightSearchValue}'";
                }
            }
            else if (func.Body is MethodCallExpression)
            {
                var methodCallExpression = func.Body as MethodCallExpression;
                if (methodCallExpression.Method.Name == "Contains")
                {
                    paramName = (methodCallExpression.Object as MemberExpression).Member.Name;
                    searchValue = (methodCallExpression.Arguments[0] as ConstantExpression).Value.ToString();
                    query = $"select * from {tableName} where {paramName} like '%{searchValue}%'";
                }
            }
            _connection.Open();
            _command.CommandText = query;
            var reader = _command.ExecuteReader();
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
            _connection.Close();
            return list;

        }
    }
}

