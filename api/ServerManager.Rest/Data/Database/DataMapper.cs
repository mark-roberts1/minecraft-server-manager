using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace ServerManager.Rest.Database
{
    /// <inheritdoc/>
    public class DataMapper : IDataMapper
    {
        /// <inheritdoc/>
        public IEnumerable<T> Map<T>(IDataReader reader) where T : new()
        {
            reader.ThrowIfNull("reader");

            if (reader.IsClosed) throw new ArgumentException("cannot read from closed reader.");
            
            var mapped = new List<T>();

            var props = GetProperties<T>();
            
            while (reader.Read())
            {
                var obj = new T();

                SetProperties(obj, props, reader);

                mapped.Add(obj);
            }

            return mapped;
        }

        /// <inheritdoc/>
        public T MapSingle<T>(IDataReader reader) where T : new()
        {
            return Map<T>(reader).FirstOrDefault();
        }

        private void SetProperties<T>(T instance, IEnumerable<PropertyInfo> properties, IDataReader reader)
        {
            foreach (var property in properties)
            {
                if (reader.TryGetValue(property, out object propertyValue))
                {
                    try
                    {
                        property.SetValue(instance, propertyValue);
                    }
                    catch (Exception ex)
                    {
                        throw new MapException(property, propertyValue, ex);
                    }
                }
            }
        }

        private IEnumerable<PropertyInfo> GetProperties<T>()
        {
            return typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        }
    }
}
