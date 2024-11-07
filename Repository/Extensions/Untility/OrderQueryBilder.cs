using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Extensions.Untility
{
    public static class OrderQueryBilder
    {
        public static string CreateOrderQuery<T>(string orderByQueryString)
        {
            var orderParams = orderByQueryString.Trim().Split(',');
            var propertyinfo = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var orderQueryBilder = new StringBuilder();
            foreach (var param in orderParams)
            {
                if (string.IsNullOrEmpty(param))
                {
                    continue;
                }
                var propertyFromQueryName = param.Trim().Split(' ')[0];
                var objectProperty = propertyinfo.FirstOrDefault(p => p.Name.Equals(propertyFromQueryName, StringComparison.CurrentCultureIgnoreCase));
                if (objectProperty == null)
                {
                    continue;
                }
                var direction = param.EndsWith(" desc") ? "descending" : "ascending";
                orderQueryBilder.Append($"{objectProperty.Name.ToString()} {direction}");
            }
            var orderQuery = orderQueryBilder.ToString().TrimEnd(',', ' ');
            return orderQuery;
        }
    }
}