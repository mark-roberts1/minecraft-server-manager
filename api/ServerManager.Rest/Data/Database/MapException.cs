using System;
using System.Reflection;
using System.Collections.Generic;
using System.Text;

namespace ServerManager.Rest.Database
{
    public class MapException : Exception
    {
        public MapException(PropertyInfo property, object value, Exception ex) 
            : base($"Error mapping {value} to {property.DeclaringType.Name}.{property.Name} {ex.Message}", ex)
        {
        }
    }
}
