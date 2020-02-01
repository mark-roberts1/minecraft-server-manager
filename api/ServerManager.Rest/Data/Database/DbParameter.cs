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
            return new DbParameter
            {
                Name = name,
                Value = value
            };
        }

        /// <summary>
        /// Constructs a DbParameter using the provided parameters
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbType"></param>
        /// <param name="typeName"></param>
        public static DbParameter From(string name, object value, SqlDbType? dbType, string typeName)
        {
            return new DbParameter
            {
                Name = name,
                Value = value,
                DbType = dbType,
                TypeName = typeName
            };
        }
    }
}
