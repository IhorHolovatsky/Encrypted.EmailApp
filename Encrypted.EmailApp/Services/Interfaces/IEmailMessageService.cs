using System.Collections.Generic;
using System.Threading.Tasks;
using Encrypted.EmailApp.Domain;

namespace Encrypted.EmailApp.Services.Interfaces
{
    public interface IEmailMessageService
    {
        Task<List<EmailMessage>> GetMessagesAsync();

        Task<EmailMessage> GetMessageByIdAsync(int messageId);

        Task<EmailMessage> SendMessageAsync(EmailMessage message, string encryptionKey);

        Task<string> DecryptMessageAsync(string text, string decryptionKey);
    }
}