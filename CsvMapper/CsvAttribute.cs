using System;

namespace Csv
{
    public class CsvAttribute : Attribute
    {
        public string Name { get; set; }
        public CsvAttribute()
        { }
        public CsvAttribute(string name) : this()
        {
            this.Name = name;
        }
    }
}
