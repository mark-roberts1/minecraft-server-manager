using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Data
{
    public static class UserCommands
    {
        public const string InsertUser = @"
            INSERT INTO User (
                Username,
                Email,
                UserRole,
                IsLocked,
                PasswordHash
            ) VALUES (
                $Username,
                $Email,
                $UserRole,
                $IsLocked,
                $PasswordHash
            )";

        public const string DeleteUser = @"DELETE FROM User WHERE UserId = $UserId";

        public const string GetUserByUsername = @"
            SELECT
                UserId,
                Username,
                MinecraftUsername,
                Email,
                UserRole,
                IsLocked
            FROM
                User
            WHERE
                Username = $Username";

        public const string GetUserById = @"
            SELECT
                UserId,
                Username,
                MinecraftUsername,
                Email,
                UserRole,
                IsLocked
            FROM
                User
            WHERE
                UserId = $UserId";

        public const string GetUserByLink = @"
            SELECT
                UserId,
                Username,
                MinecraftUsername,
                Email,
                UserRole,
                IsLocked
            FROM
                User u
                INNER JOIN ResetPasswordLink l
                    ON u.UserId = l.UserId
            WHERE
                l.Link = $Link
        ";

        public const string GetUserBySessionToken = @"
            SELECT
                UserId,
                Username,
                MinecraftUsername,
                Email,
                UserRole,
                IsLocked
            FROM
                User u
                INNER JOIN UserSession s
                    ON u.UserId = s.UserId
            WHERE
                s.Token = $Token
        ";

        public const string GetUsers = @"
            SELECT
                UserId,
                Username,
                MinecraftUsername,
                Email,
                UserRole,
                IsLocked
            FROM
                User
        ";

        public const string IsLinkValid = @"
            SELECT
                Link
            FROM
                ResetPasswordLink l
            WHERE
                l.Link = $Link
            UNION
            SELECT
                Link
            FROM
                UserInvitationLink uil
            WHERE
                uil.Link = $Link
        ";

        public const string GetUsernamePasswordHash = @"SELECT Username, PasswordHash FROM User WHERE Username = $Username";

        public const string InsertUserSessionToken = @"INSERT INTO UserSession ( UserId, Token ) VALUES ( $UserId, $Token )";

        public const string InsertInvitationLink = @"INSERT INTO UserInvitationLink ( Email, Link ) VALUES ( $Email, $Link )";

        public const string InsertResetPasswordLink = @"INSERT INTO ResetPasswordLink ( UserId, Link ) VALUES ( $UserId, $Link )";

        public const string UpdateUserLock = @"
            UPDATE
                User
            SET
                IsLocked = CASE IsLocked WHEN 1 THEN 0 ELSE 1 END
            WHERE
                UserId = $UserId

            SELECT IsLocked FROM User WHERE UserId = $UserId
        ";

        public const string UpdateUserEmail = @"
            UPDATE
                User
            SET
                Email = $Email
            WHERE
                UserId = $UserId
        ";

        public const string UpdateUserPasswordHash = @"
            UPDATE
                User
            SET
                PasswordHash = $PasswordHash
            WHERE
                UserId = $UserId
        ";

        public const string UpdateUserRole = @"
            UPDATE
                User
            SET
                UserRole = $UserRole
            WHERE
                UserId = $UserId
        ";
    }
}
