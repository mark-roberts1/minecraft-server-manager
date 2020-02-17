using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MailKit;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using ServerManager.Rest.Dto;
using ServerManager.Rest.Logging;

namespace ServerManager.Rest.Utility
{
    public class LinkGenerator : ILinkGenerator
    {
        private readonly string _smtpServer;
        private readonly string _senderEmail;
        private readonly string _resetPasswordPrefix;
        private readonly string _createAccountPrefix;
        private readonly string _smtpUser;
        private readonly string _smtpPassword;

        private readonly ILogger _logger;

        public LinkGenerator(ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            _logger = loggerFactory.GetLogger<LinkGenerator>();
            _smtpServer = configuration.GetValue<string>("Smtp:Server");
            _senderEmail = configuration.GetValue<string>("Smtp:User");
            _resetPasswordPrefix = configuration.GetValue<string>("LinkPrefixes:ForgotPassword");
            _createAccountPrefix = configuration.GetValue<string>("LinkPrefixes:CreateAccount");
            _smtpUser = configuration.GetValue<string>("Smtp:User");
            _smtpPassword = configuration.GetValue<string>("Smtp:Password");
        }

        public string GenerateUniqueLink()
        {
            return Guid.NewGuid().ToString("N");
        }

        public async Task<bool> SendInvitationLink(string email, string link, CancellationToken cancellationToken)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Minecraft Server Manager", _senderEmail));
            message.To.Add(new MailboxAddress(email));
            message.Subject = "Invitation to join";

            message.Body = new TextPart
            {
                Text = $"You have been invited to create an account on https://marksgamedomain.net.\n" +
                    "Please navigate to the site, click the \"Create Account\" option and use the below secret key to create your account.\n\n"+ 
                    $"{_createAccountPrefix}{link}"
            };

            return await SendMessage(message, cancellationToken);
        }

        public async Task<bool> SendResetPasswordLink(User user, string link, CancellationToken cancellationToken)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Minecraft Server Manager", _senderEmail));
            message.To.Add(new MailboxAddress(user.Username, user.Email));
            message.Subject = "Password Reset request";

            message.Body = new TextPart("plain")
            {
                Text = $"Please use the provided secret key to reset your password.\n\n{link}"
            };

            return await SendMessage(message, cancellationToken);
        }

        private async Task<bool> SendMessage(MimeMessage message, CancellationToken cancellationToken)
        {
            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    await client.ConnectAsync(_smtpServer, 587, false, cancellationToken);

                    await client.AuthenticateAsync(_smtpUser, _smtpPassword, cancellationToken);

                    client.Send(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex);

                return false;
            }

            return true;
        }
    }
}
