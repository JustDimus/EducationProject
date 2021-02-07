using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Reflection;
using System.Linq;
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Data.Common;
using System.Linq.Expressions;

namespace WorkWithADO
{
    public class ADOAdapter<T>
    {
        private string _connectionString;

        public ADOAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        private string GenerateCreateParameters(T entity, SqlParameterCollection parameters)
        {
            var notNullProperties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(entity)).Where(p => p.Value != null);

            string sqlQuery = $"INSERT INTO {typeof(T).Name}"
                + $"({String.Join(", ", notNullProperties.Select(p => p.Key))}) "
                + $"VALUES({String.Join(", ", notNullProperties.Select(p => $"@{p.Key}"))})";

            parameters.AddRange(notNullProperties.Select(p => new SqlParameter($"@{p.Key}", p.Value)).ToArray());

            return sqlQuery;
        }

        private string GenerateGetParameters(SqlParameterCollection parameters, params Expression<Func<T, bool>>[] expression)
        {
            string sqlQuery = $"SELECT * FROM {typeof(T).Name}";

            if (expression.Select(p => p.Body).OfType<BinaryExpression>().Any())
            {
                IEnumerable<string> expressions = Enumerable.Empty<string>();
                int counter = 1;

                foreach (var exp in expression.Select(p => p.Body).OfType<BinaryExpression>())
                {
                    expressions = expressions.Append($"{((MemberExpression)exp.Left).Member.Name} {(exp.NodeType == ExpressionType.Equal ? "=" : "!=")} @{((MemberExpression)exp.Left).Member.Name}{counter}");

                    parameters.Add(new SqlParameter($"@{((MemberExpression)exp.Left).Member.Name}{counter}", ((ConstantExpression)exp.Right).Value));

                    counter++;
                }

                sqlQuery += $" WHERE {String.Join(" AND ", expressions)}";
            }

            return sqlQuery;
        }

        private string GenerateUpdateParameters(T setEntity, SqlParameterCollection parameters, params Expression<Func<T, bool>>[] expression)
        {
            var setProperties = typeof(T).GetProperties()
                .ToDictionary(p => p.Name, p => p.GetValue(setEntity));

            string sqlQuery = $"UPDATE {typeof(T).Name} SET {String.Join(", ", setProperties.Select(p => $"{p.Key} = @{p.Key}S"))}";

            parameters.Add(setProperties.Select(p => new SqlParameter($"@{p.Key}S", p.Value)).ToArray());

            if (expression.Select(p => p.Body).OfType<BinaryExpression>().Any())
            {
                IEnumerable<string> expressions = Enumerable.Empty<string>();
                int counter = 1;

                foreach (var exp in expression.Select(p => p.Body).OfType<BinaryExpression>())
                {
                    expressions = expressions.Append($"{((MemberExpression)exp.Left).Member.Name} {(exp.NodeType == ExpressionType.Equal ? "=" : "!=")} @{((MemberExpression)exp.Left).Member.Name}{counter}");

                    parameters.Add(new SqlParameter($"@{((MemberExpression)exp.Left).Member.Name}{counter}", ((ConstantExpression)exp.Right).Value));

                    counter++;
                }

                sqlQuery += $" WHERE {String.Join(" AND ", expressions)}";
            }

            return sqlQuery;
        }

        private string GenerateDeleteParameters(SqlParameterCollection parameters, params Expression<Func<T, bool>>[] expression)
        {
            string sqlQuery = $"DELETE FROM {typeof(T).Name}";

            if (expression.Select(p => p.Body).OfType<BinaryExpression>().Any())
            {
                IEnumerable<string> expressions = Enumerable.Empty<string>();
                int counter = 1;

                foreach(var exp in expression.Select(p => p.Body).OfType<BinaryExpression>())
                {
                    expressions = expressions.Append($"{((MemberExpression)exp.Left).Member.Name} {(exp.NodeType == ExpressionType.Equal? "=" : "!=")} @{((MemberExpression)exp.Left).Member.Name}{counter}");

                    parameters.Add(new SqlParameter($"@{((MemberExpression)exp.Left).Member.Name}{counter}", ((ConstantExpression)exp.Right).Value));

                    counter++;
                }

                sqlQuery += $" WHERE {String.Join(" AND ", expressions)}";
            }

            return sqlQuery;
        }

        public void Create(T entity)
        {
            if(entity is null)
            {
                throw new ArgumentNullException();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                    
                command.CommandText = GenerateCreateParameters(entity, command.Parameters);

                command.ExecuteNonQuery();

                connection.Close();
            }

        }

        public IEnumerable<T> Get(params Expression<Func<T, bool>>[] expression)
        {
            IEnumerable<T> result = Enumerable.Empty<T>();

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = GenerateGetParameters(command.Parameters, expression);

                foreach (DbDataRecord row in command.ExecuteReader())
                {
                    T newEnitiy = (T)Activator.CreateInstance(typeof(T));

                    foreach (var property in typeof(T).GetProperties())
                    {
                        property.SetValue(newEnitiy, row[property.Name] is DBNull? null : row[property.Name]);
                    }

                    result = result.Append(newEnitiy);
                }
            }

            return result;
        }

        public void Update(T setEntity, params Expression<Func<T, bool>>[] expression)
        {
            if(setEntity is null || expression.Any() == false)
            {
                throw new ArgumentNullException();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = GenerateUpdateParameters(setEntity, command.Parameters, expression);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }

        public void Delete(params Expression<Func<T, bool>>[] expression)
        {
            if(expression.Any() == false)
            {
                throw new ArgumentNullException();
            }

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = GenerateDeleteParameters(command.Parameters, expression);

                command.ExecuteNonQuery();

                connection.Close();
            }
        }
    }
}
