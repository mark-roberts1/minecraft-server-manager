using Microsoft.AspNetCore.Mvc;
using ServerManager.Rest.Data;
using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServerManager.Rest.Controllers
{
    public abstract class ApiController : ControllerBase
    {
        public ApiController(IUserData userData)
        {
            UserData = userData;
        }

        protected bool IsAuthenticated
        {
            get
            {
                var task = GetAuthenticatedUser();
                task.Wait();

                return task.Result != null;
            }
        }

        protected async Task<bool> IsAuthenticatedAsync()
        {
            return (await GetAuthenticatedUser()) != null;
        }

        protected bool IsLocked
        {
            get
            {
                var task = GetAuthenticatedUser();
                task.Wait();

                return task.Result?.IsLocked ?? false;
            }
        }

        protected bool IsAdmin
        {
            get
            {
                var task = GetAuthenticatedUser();
                task.Wait();

                return (task.Result?.UserRole ?? UserRole.Normal) == UserRole.Admin;
            }
        }

        protected IUserData UserData { get; }

        [FromHeader(Name = "SessionToken")]
        public string SessionToken { get; set; }

        private User userCache;
        private bool haveCheckedDb = false;

        protected async Task<User> GetAuthenticatedUser()
        {
            if (userCache == null && !haveCheckedDb)
            {
                userCache = await UserData.GetUserBySessionTokenAsync(SessionToken, default);

                haveCheckedDb = true;
            }

            return userCache;
        }

        protected void ThrowIfUnauthenticated()
        {
            if (!IsAuthenticated)
            {
                throw new UnauthorizedAccessException("User must be authenticated to use this method.");
            }

            if (IsLocked)
            {
                throw new UnauthorizedAccessException("Your account has been locked");
            }
        }

        protected void ThrowIfNotAdmin()
        {
            ThrowIfUnauthenticated();

            if (!IsAdmin)
            {
                throw new UnauthorizedAccessException();
            }
        }

        protected async Task ThrowIfNotAdminOrNotAuthenticatedUser(int userId)
        {
            ThrowIfUnauthenticated();

            if (!IsAdmin && userId != (await GetAuthenticatedUser()).UserId)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
