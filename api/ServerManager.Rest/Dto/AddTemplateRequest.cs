using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Dto
{
    public class AddTemplateRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string DownloadLink { get; set; }
    }
}
