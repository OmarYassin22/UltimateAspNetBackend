using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel;
using System.Reflection;

namespace Api_New_Feature.ModelBinders
{
    public class ArrayModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            // Our binder works only on enumerable types
            if (!bindingContext.ModelMetadata.IsEnumerableType)
            {
                bindingContext.Result = ModelBindingResult.Failed();
                return Task.CompletedTask;
            }
            // extract the value provided in the request
            var providedValue = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).ToString();
            // check if the value is null or empty
            if (string.IsNullOrWhiteSpace(providedValue))
            {
                bindingContext.Result = ModelBindingResult.Success(null);
                return Task.CompletedTask;
            }
            // get the enumerable's type, and a converter for that type "GUID"
            var genericType = bindingContext.ModelType.GetTypeInfo().GenericTypeArguments[0];

            var converter = TypeDescriptor.GetConverter(genericType);
            // splite provided value with comma and convert each item to the generic type
            var objectArray = providedValue.Split(new[] { "," },
                       StringSplitOptions.RemoveEmptyEntries)
                       .Select(x => converter.ConvertFromString(x.Trim()))
                       .ToArray();
            // create an array of the generic type 'Guid' and with length of the objectArray'valeus provided'
            var guidArray = Array.CreateInstance(genericType, objectArray.Length);
            // copy the objectArray to the guidArray
            objectArray.CopyTo(guidArray, 0);
            // set the model with the guidArray
            bindingContext.Model = guidArray;
            // set the result to success
            bindingContext.Result = ModelBindingResult.Success(bindingContext.Model);
            return Task.CompletedTask;
        }
    }
}
