using ADODataContext.Converters;
using ADODataContext.Interfaces;
using EducationProject.Core;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace ADODataContext.DataSets
{
    public class BaseDbSet<T> : IDbSet<T> where T : class
    {
        private LambdaConverter<T> converter;

        private int currentId;

        private Queue<SqlCommand> commands = new Queue<SqlCommand>();

        public BaseDbSet(string dbConnectionString, LambdaConverter<T> adoAdapter)
        {
            connectionString = dbConnectionString;

            converter = adoAdapter;

            currentId = 1;

            var elements = this.Get().Select(p => p.Id);
            if(elements.Any())
            {
                currentId = elements.Max() + 1;
            }
        }

        private string connectionString { get; set; }

        public int CurrentId()
        {
            return currentId++;
        }

        public void Create(T entity)
        {
            entity.Id = CurrentId();

            var notNullProperties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(entity)).Where(p => p.Value != null);

            string sqlQuery = $"INSERT INTO {typeof(T).Name}"
                + $"({String.Join(", ", notNullProperties.Select(p => p.Key))}) "
                + $"VALUES({String.Join(", ", notNullProperties.Select(p => $"@{p.Key}"))})";

            SqlCommand command = new SqlCommand();

            command.CommandText = sqlQuery;
            command.Parameters.AddRange(notNullProperties.Select(p => new SqlParameter($"@{p.Key}", p.Value)).ToArray());

            commands.Enqueue(command);
        }

        public void Delete(T entity)
        {
            Delete(p => p.Id == entity.Id);
        }

        public void Delete(int id)
        {
            Delete(p => p.Id == id);
        }

        public void Delete(Expression<Func<T, bool>> condition)
        {
            string sqlQuery = $"DELETE FROM {typeof(T).Name}";

            SqlCommand command = new SqlCommand();

            string whereCondition = converter.DeconvertData(condition, command.Parameters);

            if(String.IsNullOrEmpty(whereCondition) == false)
            {
                sqlQuery += " WHERE " + whereCondition;
            }

            command.CommandText = sqlQuery;

            commands.Enqueue(command);
        }

        public IEnumerable<T> Get(Expression<Func<T, bool>> condition)
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            string sqlQuery = $"SELECT * FROM {typeof(T).Name}";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                SqlCommand command = new SqlCommand();

                command.Connection = connection;

                string whereCondition = converter.DeconvertData(condition, command.Parameters);

                if (String.IsNullOrEmpty(whereCondition) == false)
                {
                    sqlQuery += " WHERE " + whereCondition;
                }

                command.CommandText = sqlQuery;

                foreach (DbDataRecord row in command.ExecuteReader())
                {
                    T newEnitiy = (T)Activator.CreateInstance(typeof(T));

                    foreach (var property in typeof(T).GetProperties())
                    {
                        property.SetValue(newEnitiy, row[property.Name] is DBNull ? null : row[property.Name]);
                    }

                    result = result.Append(newEnitiy);
                }

                connection.Close();
            }
            return result;
        }

        public T Get(int id)
        {
            return Get(prop => prop.Id == id).FirstOrDefault();
        }

        public T Get(T entity)
        {
            return Get(p => p.Id == entity.Id).FirstOrDefault();
        }

        public IEnumerable<T> Get()
        {
            return this.Get(prop => true);
        }

        public void Save()
        {
            SqlCommand command = null;

            if (commands.Any() == false)
            {
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                while (commands.TryDequeue(out command) == true)
                {
                    command.Connection = connection;

                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public void Update(T entity)
        {
            Update(entity, e => e.Id == entity.Id);
        }

        public void Update(T entity, Expression<Func<T, bool>> condition)
        {
            var setProperties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(entity));

            string sqlQuery = $"UPDATE {typeof(T).Name} SET {String.Join(", ", setProperties.Select(p => $"{p.Key} = @{p.Key}S"))}";

            SqlCommand command = new SqlCommand();

            command.Parameters.Add(setProperties.Select(p => new SqlParameter($"@{p.Key}S", p.Value)).ToArray());

            string whereCondition = converter.DeconvertData(condition, command.Parameters);

            if(String.IsNullOrEmpty(whereCondition) == false)
            {
                sqlQuery += " WHERE " + whereCondition;
            }

            command.CommandText = sqlQuery;

            commands.Enqueue(command);
        }
    }
}
