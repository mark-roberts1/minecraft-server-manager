using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data.Queries
{
    public class SchemaCommands
    {
        public const string CreateUserTable = @"
            CREATE TABLE IF NOT EXISTS User (
                UserId INTEGER PRIMARY KEY,
                Username TEXT NOT NULL UNIQUE,
                PasswordHash TEXT NOT NULL,
                MinecraftUsername TEXT UNIQUE,
                Email TEXT UNIQUE,
                UserRole INTEGER DEFAULT 0,
                IsLocked INTEGER DEFAULT 0
            )";

        public const string CreateResetPasswordLinkTable = @"
            CREATE TABLE IF NOT EXISTS ResetPasswordLink (
                ResetPasswordLinkId INTEGER PRIMARY KEY,
                UserId INTEGER NOT NULL,
                Link TEXT NOT NULL,
                FOREIGN KEY (UserId) REFERENCES User (UserId) ON DELETE CASCADE ON UPDATE NO ACTION
            )";
        
        public const string CreateUserSessionTable = @"
            CREATE TABLE IF NOT EXISTS UserSession (
                UserSessionId INTEGER PRIMARY KEY,
                UserId INTEGER NOT NULL,
                Token TEXT NOT NULL,
                FOREIGN KEY (UserId) REFERENCES User (UserId) ON DELETE CASCADE ON UPDATE NO ACTION
            )";
        
        public const string CreateUserInvtitationLinkTable = @"
            CREATE TABLE IF NOT EXISTS UserInvitationLink (
                Email TEXT NOT NULL,
                Link TEXT NOT NULL
            )";

        public const string CreateTemplateTable = @"
            CREATE TABLE IF NOT EXISTS Template (
                TemplateId INTEGER PRIMARY KEY,
                Name TEXT NOT NULL,
                Description TEXT NULL,
                Version TEXT NOT NULL UNIQUE,
                DownloadLink TEXT NOT NULL
            )";

        public const string CreateDefaultPropertiesTable = @"
            CREATE TABLE IF NOT EXISTS DefaultProperties (
                Properties TEXT NOT NULL
            )";

        public const string CreateServerTable = @"
            CREATE TABLE IF NOT EXISTS Server (
                ServerId INTEGER PRIMARY KEY,
                Name TEXT NOT NULL,
                Version TEXT NOT NULL,
                Description TEXT NULL,
                Status INTEGER DEFAULT 0
            )";

        public const string CreateServerPropertiesTable = @"
            CREATE TABLE IF NOT EXISTS ServerProperties (
                ServerId INTEGER NOT NULL UNIQUE,
                Properties TEXT NOT NULL,
                FOREIGN KEY (ServerId) REFERENCES Server (ServerId) ON DELETE CASCADE ON UPDATE NO ACTION
            )";

        public const string DefaultPropertiesValue = 
@"spawn-protection=16
max-tick-time=60000
query.port=25565
generator-settings=
force-gamemode=false
allow-nether=true
enforce-whitelist=false
gamemode=survival
broadcast-console-to-ops=true
enable-query=false
player-idle-timeout=0
difficulty=easy
spawn-monsters=true
broadcast-rcon-to-ops=true
op-permission-level=4
pvp=true
snooper-enabled=true
level-type=default
hardcore=false
enable-command-block=false
max-players=20
network-compression-threshold=256
resource-pack-sha1=
max-world-size=29999984
function-permission-level=2
rcon.port=25575
server-port=25565
server-ip=
spawn-npcs=true
allow-flight=false
level-name=world
view-distance=10
resource-pack=
spawn-animals=true
white-list=false
rcon.password=
generate-structures=true
online-mode=true
max-build-height=256
level-seed=
prevent-proxy-connections=false
use-native-transport=true
motd=A Minecraft Server
enable-rcon=true";
    }
}
