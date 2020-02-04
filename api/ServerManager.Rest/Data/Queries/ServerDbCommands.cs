using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public class ServerDbCommands
    {
        public const string InsertTemplate = @"
        ";
        public const string InsertServer = @"
        ";
        public const string DeleteServer = @"
        ";
        public const string UpdateTemplate = @"
        UPDATE
            Template
        SET
            Name = $Name
            Description = $Description
            Version = $Version
            DownloadLink = $DownloadLink
        WHERE
            TemplateId = $TemplateId
        ";
        public const string UpdateServer = @"
        UPDATE
            Server
        SET
            Name=$Name
            Version=$Version
            Description=$Description
            Properties=$Properties
        WHERE
            ServerId = $ServerId
        ";
        public const string UpdateServerStatus = @"
        UPDATE
            Server
        SET
            ServerStatus = $ServerStatus
        WHERE
            ServerId = $ServerId
        ";
        public const string ListTemplates = @"
        SELECT
            TemplateId,
            Name,
            Description,
            Version,
            DownloadLink,
            Properties
        FROM
            Templates
        ";
        public const string ListServers = @"
        SELECT
            ServerId,
            Name,
            Description,
            Version,
            Properties
        FROM
            Templates
        ";
        public const string GetTemplate = @"
        SELECT
            Name,
            Description,
            Version,
            Properties
        FROM
            Templates
        WHERE
            TemplateId = $TemplateId
        ";
    }
}
