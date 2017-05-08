using System.Reflection;

namespace Csv
{
    internal sealed class PropertyInfoPosition
    {
        public PropertyInfo PropertyInfo { get; set; }
        public int Position { get; set; }
        public bool Active { get; set; }
        internal PropertyInfoPosition(PropertyInfo propertyInfo)
        {
            PropertyInfo = propertyInfo;
        }
    }
}
