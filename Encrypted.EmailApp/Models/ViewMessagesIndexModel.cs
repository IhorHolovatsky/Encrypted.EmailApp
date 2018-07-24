using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Encrypted.EmailApp.Domain;
using Encrypted.EmailApp.Services.Interfaces;

namespace Encrypted.EmailApp.Models
{
    public class ViewMessagesIndexModel
    {
        public List<EmailMessage> Messages { get; private set; }

        private readonly IEmailMessageService _emailMessageService;

        public ViewMessagesIndexModel(IEmailMessageService emailMessageService)
        {
            if (emailMessageService == null) throw new ArgumentNullException(nameof(emailMessageService));

            _emailMessageService = emailMessageService;
        }

        public async Task BuildAsync()
        {
            Messages = await _emailMessageService.GetMessagesAsync();
        }
    }
}