using System;
using System.Data;

namespace ServerManager.Rest.Database
{
    /// <inheritdoc/>
    public class DbParameter : IParameter
    {
        /// <inheritdoc/>
        public string Name { get; set; }

        /// <inheritdoc/>
        public object Value { get; set; }

        /// <inheritdoc/>
        public SqlDbType? DbType { get; set; }

        /// <inheritdoc/>
        public string TypeName { get; set; }

        /// <summary>
        /// Constructs a DbParameter using the provided parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        public static DbParameter From(string name, object value)
        {
            if (value != null && value.GetType().IsEnum)
            {
                value = (int)value;
            }

            return new DbParameter
            {
                Name = name,
                Value = ValueOrDbNull(value)
            };
        }

        private static object ValueOrDbNull(object value)
        {
            if (value == null) return DBNull.Value;

            return value;
        }
    }
}
