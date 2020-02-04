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
    }
}
