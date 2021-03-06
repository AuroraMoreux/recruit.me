﻿namespace RecruitMe.Services.Messaging
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class NullMessageSender : IEmailSender
    {
        public Task SendEmailAsync(
            string from,
            string fromName,
            string to,
            string subject,
            string htmlContent,
            IEnumerable<EmailAttachment> attachments = null,
            params string[] additionalEmailAddresses)
        {
            return Task.CompletedTask;
        }
    }
}
