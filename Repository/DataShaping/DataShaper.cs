using Contracts;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Repository.DataShaping
{
    public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] properties { get; set; }
        public DataShaper()
        {
            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        }
        public IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fields)
        {
            var reuiredProps = GetRequiredProperty(fields);
            return ShapedData(entities, reuiredProps);
        }



        public ExpandoObject ShapeData(T entity, string stringFields)
        {
            var requiredProperties = GetRequiredProperty(stringFields);
            return ShapedDataForEntity(entity, requiredProperties);
        }

        private ExpandoObject ShapedDataForEntity(T entity, List<PropertyInfo> requiredProperties)
        {
            var shapedObject = new ExpandoObject();
            foreach (var prop in requiredProperties)
            {
                var value = prop.GetValue(entity);
                shapedObject.TryAdd(prop.Name, prop.GetValue(entity));

            }
            return shapedObject;
        }

        private List<PropertyInfo> GetRequiredProperty(string stringFields)
        {
            var requiredProperties = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(stringFields))
            {
                var fields = stringFields.Split(',', StringSplitOptions.RemoveEmptyEntries);
                foreach (var field in fields)
                {
                    var prop = properties.FirstOrDefault(p => p.Name.Equals(field.Trim(), StringComparison.CurrentCultureIgnoreCase));
                    if (prop is null)
                        continue;
                    requiredProperties.Add(prop);
                }

            }
            else { requiredProperties = properties.ToList(); }
            return requiredProperties;
        }
        private IEnumerable<ExpandoObject> ShapedData(IEnumerable<T> entities, List<PropertyInfo> reuiredProps)
        {
            var shapedObject = new List<ExpandoObject>();
            foreach (var entity in entities)
            {
                shapedObject.Add(ShapedDataForEntity(entity, reuiredProps));
            }
            return shapedObject;
        }
    }
}
