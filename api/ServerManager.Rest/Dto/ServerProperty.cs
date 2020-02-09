using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ServerProperty
    {
        public ServerProperty() { }
        
        public string Key { get; set; }
        
        private string value;

        public string Value 
        { 
            get => this.value; 
            set => this.value = value; 
        }

        public static implicit operator ServerProperty(string fileLine)
        {
            if (string.IsNullOrWhiteSpace(fileLine)) throw new InvalidCastException("Invalid server property");
            if (fileLine.Trim().StartsWith("#")) throw new InvalidCastException("ServerProperty commented out");
            var vals = fileLine.Replace("\r", "").Split('=');

            var prop = new ServerProperty();

            prop.Key = vals[0];

            if (vals.Length > 1) prop.Value = vals[1];

            return prop;
        }

        public override string ToString()
        {
            return string.Join('=', Key, Value);
        }
    }
}
