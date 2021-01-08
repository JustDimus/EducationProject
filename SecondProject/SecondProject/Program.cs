using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections;

namespace SecondProject
{
    class Program
    {
        static void Main(string[] args)
        {
            //a
            var first = TTR.customers.OrderBy(p => p.RegistrationDate).FirstOrDefault();

            //b
            var secondAvg = TTR.customers.Select(c => c.Balance).Average();

        }

        //c
        static List<Customer> GetCustomersBetween(DateTime start, DateTime end)
        {
            var result = TTR.customers.Where(c => c.RegistrationDate >= start && c.RegistrationDate <= end).ToList();

            if (result.Any())
            {
                Log("No results");
            }

            return result;
        }

        //d
        static List<Customer> GetById(IEnumerable<long> IdList)
        {
            return TTR.customers.Where(c => IdList.Contains(c.Id)).ToList();
        }

        //e
        static List<Customer> GetByName(string Substr)
        {
            string substr = Substr.ToLower();

            return TTR.customers.Where(c => c.Name.ToLower().Contains(substr)).ToList();
        }

        //f
        static List<IGrouping<int, Customer>> GetGroupBy()
        {
            return TTR.customers.OrderBy(c => c.RegistrationDate.Month).
                ThenBy(c => c.Name).GroupBy(c => c.RegistrationDate.Month).ToList();
        }

        //g
        static List<Customer> GetByFiled(string FieldName, bool isDesc = false)
        {
            List<Customer> result = new List<Customer>();

            Type type = typeof(Customer);

            PropertyInfo property = type.GetProperties().Where(p => p.Name == FieldName).FirstOrDefault();

            if (property == null)
                throw new Exception();

            return TTR.customers.OrderBy(c => property.GetValue(c),
                isDesc ? (IComparer<object>)new FieldComparerDesc()
                : (IComparer<object>)new FieldComparerAsc()).ToList();
        }

        //h
        static string GetAllNames()
        {
            return String.Join(", ", TTR.customers.Select(c => c.Name));
        }


        private class FieldComparerAsc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                return ((IComparable)x).CompareTo((IComparable)y);
            }
        }

        private class FieldComparerDesc : IComparer<object>
        {
            public int Compare(object x, object y)
            {
                return ((IComparable)y).CompareTo((IComparable)x);
            }
        }

        static void Log(string message)
        {
            Console.WriteLine(message);
        }

    }
}
