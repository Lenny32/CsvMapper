using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Csv
{
    public class CsvMapper<T>
        where T : new()
    {
        private string[] _csvRows;

        private IDictionary<string, PropertyInfoPosition> _propertyInformations;
        private readonly char _separator;
        private StreamReader _stream;
        private string _headerRow;
        private int _position = 0;
        private string GetNextRow()
        {
            if (_stream == null)
            {
                return _position < _csvRows.Length ? _csvRows[_position++] : null;
            }
            return _stream.ReadLine()?.Replace("\r", String.Empty);
        }
        private CsvMapper(char separator = ',')
        {
            _propertyInformations = new Dictionary<string, PropertyInfoPosition>();
            Init();
            this._separator = separator;
        }

        public CsvMapper(string text, char separator = ',') : this(separator)
        {
            _csvRows = text.Replace("\r", string.Empty).Split('\n');
            this._headerRow = _csvRows[0];
            _csvRows = _csvRows.Skip(1).ToArray();
        }

        public CsvMapper(Stream stream, char separator = ',') : this(separator)
        {
            var sr = new StreamReader(stream, true);
            this._headerRow = sr.ReadLine();
            this._stream = sr;
        }




        public IEnumerable<T> Parse()
        {
            ParseHeader();

            var resultList = new List<T>();
            string row = null;
            while ((row = this.GetNextRow()) != null)
            {
                var newObject = new T();
                var data = row;
                var regex = new Regex($"(\"[^ \"]*\" |[^;]+)");
                var dataColumns = regex.Matches(data);

                foreach (var prop in _propertyInformations)
                {
                    var rawValue = dataColumns[prop.Value.Position].Value;
                    //TODO Cast here
                    rawValue = rawValue.Trim('\"');

                    var value = Convert.ChangeType(rawValue, prop.Value.PropertyInfo.PropertyType);

                    prop.Value.PropertyInfo.SetValue(newObject, value);

                }
                resultList.Add(newObject);
            }


            return resultList;
        }
        private void ParseHeader()
        {
            var header = this._headerRow;
            var columns = header.Split(_separator);

            for (int i = 0; i < columns.Length; i++)
            {
                var columnName = columns[i].Trim('\"');
                if (_propertyInformations.ContainsKey(columnName))
                {
                    var property = _propertyInformations[columnName];
                    property.Position = i;
                    property.Active = true;
                }
                else
                {
                    _propertyInformations.Remove(columnName);
                }
            }

            for (int i = _propertyInformations.Keys.Count - 1; i >= 0; i--)
            {
                var key = _propertyInformations.Keys.ElementAt(i);
                if (!_propertyInformations[_propertyInformations.Keys.ElementAt(i)].Active)
                {
                    _propertyInformations.Remove(key);
                }
            }

        }
        private void Init()
        {
            foreach (var property in typeof(T).GetRuntimeProperties())
            {
                var csvAttribute = property.GetCustomAttribute<CsvAttribute>(true);

                _propertyInformations.Add(csvAttribute?.Name ?? property.Name, new PropertyInfoPosition(property));
            }
        }

    }
}
