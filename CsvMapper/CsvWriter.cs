using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Csv
{
    public class CsvWriter<T>
    {
        private readonly char _separator;
        private IDictionary<string, PropertyInfoPosition> _propertyInformations;

        public CsvWriter(char separator)
        {
            this._separator = separator;
            _propertyInformations = new Dictionary<string, PropertyInfoPosition>();
            Init();
        }

        public async Task<string> RunAsync(IEnumerable<T> list)
        {
            var properties = _propertyInformations.OrderBy(x => x.Value.Position);
            using (var writer = new StringWriter())
            {
                //Write header
                for (int i = 0; i < properties.Count(); i++)
                {
                    var property = properties.ElementAt(i);
                    await writer.WriteAsync($"{property.Key}{(i + 1 < properties.Count() ? $"{_separator}" : "")}");
                }
                await writer.WriteAsync("\r\n");

                //Write Body
                foreach (var item in list)
                {
                    for (int i = 0; i < properties.Count(); i++)
                    {
                        var property = properties.ElementAt(i);
                        await writer.WriteAsync($"{property.Value.PropertyInfo.GetValue(item)}{(i + 1 < properties.Count() ? $"{_separator}" : "")}");
                    }
                    await writer.WriteAsync("\r\n");
                }
                return writer.ToString();
            }
                
        }

        private void Init()
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                var csvAttribute = property.GetCustomAttribute<CsvAttribute>(true);
                
                _propertyInformations.Add(csvAttribute?.Name ?? property.Name, new PropertyInfoPosition(property) { Position = csvAttribute.Position });
            }
        }
    }
}
