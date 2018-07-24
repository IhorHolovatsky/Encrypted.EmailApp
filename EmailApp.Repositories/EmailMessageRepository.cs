using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailApp.Domain;
using EmailApp.Domain.Interfaces;

namespace EmailApp.Repositories
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        private static List<EmailMessage> _messages = new List<EmailMessage>();

        static EmailMessageRepository()
        {
            _messages.Add(new EmailMessage
            {
                Message = "Test",
                Subject = "Test subj",
                FromUsername = "Ihor",
                MessageId = 1,
                UserId = 5
            });
        }
        public EmailMessageRepository()
        {
            
        }

        public Task<List<EmailMessage>> GetMessagesAsync()
        {
            return Task.FromResult(_messages);
        }

        public Task SendMessageAsync(EmailMessage message)
        {
            var random = new Random();
            message.MessageId = random.Next(0, int.MaxValue);
            _messages.Add(message);

            return Task.FromResult(0);
        }

        public Task<EmailMessage> GetMessageByIdAsync(int messageId)
        {
            return Task.FromResult(_messages.FirstOrDefault(m => m.MessageId == messageId));
        }
    }
}