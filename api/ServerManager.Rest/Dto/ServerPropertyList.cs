using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class ServerPropertyList : List<ServerProperty>
    {
        public ServerPropertyList() { }

        public ServerPropertyList(IEnumerable<ServerProperty> props)
        {
            AddRange(props);
        }

        public new void Add(ServerProperty serverProperty)
        {
            if (serverProperty == null) throw new ArgumentNullException("serverProperty");

            if (this.Any(w => w.Key == serverProperty.Key)) 
                throw new InvalidOperationException($"Cannot insert duplicate key \"{serverProperty.Key}\"");
            
            base.Add(serverProperty);
        }

        public new void AddRange(IEnumerable<ServerProperty> props)
        {
            if (props == null) throw new ArgumentNullException("props");

            foreach (var prop in props) Add(prop);
        }

        public static implicit operator ServerPropertyList(string[] lines)
        {
            if (lines == null) throw new ArgumentNullException("lines");
            
            var list = new ServerPropertyList();

            foreach (var line in lines)
            {
                try
                {
                    list.Add(line);
                }
                catch { }
            }

            return list;
        }

        internal int RconPort
        {
            get
            {
                var prop = this.First(w => w.Key == "rcon.port");

                return int.Parse(prop.Value?.Trim());
            }
        }

        internal string RconPassword
        {
            get
            {
                var prop = this.First(w => w.Key == "rcon.password");

                return prop.Value?.Trim();
            }
        }

        internal bool RconEnabled
        {
            get
            {
                var prop = this.FirstOrDefault(w => w.Key == "enable-rcon");

                if (prop == null) return false;

                return bool.Parse(prop.Value?.Trim());
            }
        }
    }
}
