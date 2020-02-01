using System.Data;

namespace ServerManager.Rest.Database
{
    /// <summary>
    /// Parameter to provide to an instance of <see cref="ICommand"/>
    /// </summary>
    public interface IParameter
    {
        /// <summary>
        /// Name of the parameter
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Value of the parameter
        /// </summary>
        object Value { get; }
        /// <summary>
        /// Data type of the parameter
        /// </summary>
        SqlDbType? DbType { get; }
        /// <summary>
        /// For structured types, the name of the user defined type
        /// </summary>
        string TypeName { get; }
    }
}
