using Entities.models;
using Repository.Extensions.Untility;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;


namespace Repository.Extensions
{
    internal static class RepositoryEmployeeExtensions
    {
        public static IQueryable<Employee> FilterEmployee(this IQueryable<Employee> employees, uint minAge, uint maxAge)

            => employees.Where(e => e.Age >= minAge && e.Age <= maxAge);

        public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
            {
                return employees;
            }
            var lowerTerm = searchTerm.Trim().ToLower();
            return employees.Where(e => e.Name.ToLower().Contains(lowerTerm));
        }
        public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string OrderByQueryString)
        {
            if (string.IsNullOrWhiteSpace(OrderByQueryString))
                return employees.OrderBy(e => e.Name);
            var orderQuery = OrderQueryBilder.CreateOrderQuery<Employee>(OrderByQueryString);
            if (string.IsNullOrWhiteSpace(orderQuery))
                return employees.OrderBy(e => e.Name);
            var res= employees.OrderBy(orderQuery);
            return res;
        }
    }
}
