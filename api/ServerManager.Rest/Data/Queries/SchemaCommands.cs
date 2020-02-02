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

    }
}
