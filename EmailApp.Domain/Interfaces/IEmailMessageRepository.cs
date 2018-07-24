using System.Collections.Generic;
using System.Threading.Tasks;

namespace EmailApp.Domain.Interfaces
{
    public interface IEmailMessageRepository
    {
        Task<List<EmailMessage>> GetMessagesAsync();
        Task<EmailMessage> GetMessageByIdAsync(int messageId);

        Task SendMessageAsync(EmailMessage message);
    }
}