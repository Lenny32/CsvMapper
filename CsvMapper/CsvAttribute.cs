using System;

namespace Csv
{
    public class CsvAttribute : Attribute
    {
        public string Name { get; set; }
        public int Position { get; set; }

        public CsvAttribute()
        { }
        public CsvAttribute(string name) : this()
        {
            this.Name = name;
        }
        public CsvAttribute(int position) : this()
        {
            this.Position = position;
        }

        public CsvAttribute(string name, int position) : this()
        {
            this.Name = name;
            this.Position = position;
        }
    }
}
