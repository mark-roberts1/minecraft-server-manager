using ServerManager.Rest.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ServerManager.Rest.Utility
{
    public interface ILinkGenerator
    {
        string GenerateUniqueLink();
        Task<bool> SendResetPasswordLink(User user, string link, CancellationToken cancellationToken);
        Task<bool> SendInvitationLink(string email, string link, CancellationToken cancellationToken);
    }
}
