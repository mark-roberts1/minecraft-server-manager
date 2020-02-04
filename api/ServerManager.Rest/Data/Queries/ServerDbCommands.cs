using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public class ServerDbCommands
    {
        public const string InsertTemplate = @"
            INSERT INTO Template (
                Name,
                Description,
                Version,
                DownloadLink
            ) VALUES (
                $Name,
                $Description,
                $Version,
                $DownloadLink
            );

            SELECT last_insert_rowid();
        ";

        public const string InsertServer = @"
            INSERT INTO Server (
                Name,
                Version,
                Description
            ) VALUES (
                $Name,
                $Version,
                $Description
            );

            SELECT last_insert_rowid();
        ";

        public const string InsertServerProperties = @"
            INSERT INTO ServerProperties (
                ServerId,
                Properties
            ) VALUES (
                $ServerId,
                $Properties
            );
        ";

        public const string DeleteServer = @"DELETE FROM Server WHERE ServerId = $ServerId";

        public const string SelectServer = @"
            SELECT
                s.ServerId,
                Name,
                Version,
                Description,
                Status,
                Properties
            FROM
                Server s
                INNER JOIN ServerProperties sp
                    ON s.ServerId = sp.ServerId
            WHERE
                s.ServerId = $ServerId;";
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
