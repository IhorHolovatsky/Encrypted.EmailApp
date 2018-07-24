using System.Collections.Generic;
using System.Threading.Tasks;
using Encrypted.EmailApp.Domain;
using Encrypted.EmailApp.Encryption;
using Encrypted.EmailApp.Repositories.Interfaces;
using Encrypted.EmailApp.Services.Interfaces;

namespace Encrypted.EmailApp.Services
{
    public class EmailMessageService : IEmailMessageService
    {
        private readonly IEmailMessageRepository _messageRepository;

        public EmailMessageService(IEmailMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public Task<List<EmailMessage>> GetMessagesAsync()
        {
            return _messageRepository.GetMessagesAsync();
        }

        public Task<EmailMessage> SendMessageAsync(EmailMessage message, string encryptionKey)
        {
            //Encryption by DES then AES...
            //more encryption :)
            message.Message = MailMessageEncryption.EncryptText(message.Message, encryptionKey);

            return _messageRepository.SendMessageAsync(message);
        }

        public async Task<EmailMessage> GetMessageByIdAsync(int messageId)
        {
            var message = await _messageRepository.GetMessageByIdAsync(messageId);
            
            return message;
        }

        public Task<string> DecryptMessageAsync(string text, string decryptionKey)
        {
            var decryptedText = MailMessageEncryption.DecryptText(text, decryptionKey);

            return Task.FromResult(decryptedText);
        }
    }
}