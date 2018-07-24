using System.Collections.Generic;
using System.Threading.Tasks;
using Encrypted.EmailApp.Domain;

namespace Encrypted.EmailApp.Repositories.Interfaces
{
    public interface IEmailMessageRepository
    {
        Task<List<EmailMessage>> GetMessagesAsync();
        Task<EmailMessage> GetMessageByIdAsync(int messageId);

        Task<EmailMessage> SendMessageAsync(EmailMessage message);
    }
}