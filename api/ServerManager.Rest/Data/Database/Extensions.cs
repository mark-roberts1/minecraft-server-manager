using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Extension methods
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Ensures that a string ends with ".sql"
        /// </summary>
        /// <param name="filename">string to check</param>
        public static string EnsureSqlFile(this string filename)
        {
            if (filename == null || filename.EndsWith(".sql")) return filename;

            return $"{filename}.sql";
        }

        /// <summary>
        /// Attempts to read a value from a DataReader.
        /// </summary>
        /// <param name="reader">a DataReader</param>
        /// <param name="property">Contains the name to use to identify the value in the DataReader</param>
        /// <param name="value">out param use to assign the value</param>
        /// <returns><see langword="true"/> if value was found, <see langword="false"/> if not.</returns>
        public static bool TryGetValue(this IDataReader reader, PropertyInfo property, out object value)
        {
            try
            {
                int index = reader.GetOrdinal(property.Name);

                if (index < 0) throw new ArgumentException();

                if (reader.IsDBNull(index))
                {
                    value = null;
                    return true;
                }

                if (property.PropertyType == typeof(Boolean))
                    value = reader.GetBoolean(index);
                else if (property.PropertyType == typeof(Byte))
                    value = reader.GetByte(index);
                else if (property.PropertyType == typeof(Char))
                    value = reader.GetChar(index);
                else if (property.PropertyType == typeof(DateTime))
                    value = reader.GetDateTime(index);
                else if (property.PropertyType == typeof(Double))
                    value = reader.GetDouble(index);
                else if (property.PropertyType == typeof(float))
                    value = reader.GetFloat(index);
                else if (property.PropertyType == typeof(Guid))
                    value = reader.GetGuid(index);
                else if (property.PropertyType == typeof(Int16))
                    value = reader.GetInt16(index);
                else if (property.PropertyType == typeof(Int32))
                    value = reader.GetInt32(index);
                else if (property.PropertyType == typeof(Int64))
                    value = reader.GetInt64(index);
                else if (property.PropertyType == typeof(String))
                    value = reader.GetString(index);
                //else if (property.PropertyType == typeof())
                else
                    value = reader.GetValue(index);

                return true;
            }
            catch
            {
                value = default;
                return false;
            }
        }
    }
}
