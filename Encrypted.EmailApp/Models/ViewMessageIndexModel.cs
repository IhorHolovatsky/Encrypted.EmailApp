using System.Threading.Tasks;
using Encrypted.EmailApp.Services.Interfaces;

namespace Encrypted.EmailApp.Models
{
    public class ViewMessageIndexModel
    {
        public EmailMessageViewModel Message { get; set; }

        public string EncryptionKey { get; set; }

        private readonly IEmailMessageService _emailMessageService;
        public ViewMessageIndexModel(IEmailMessageService emailMessageService)
        {
            _emailMessageService = emailMessageService;
        }

        public async Task BuildAsync(int messageId)
        {
            var message = await _emailMessageService.GetMessageByIdAsync(messageId);

            Message = new EmailMessageViewModel
            {
                MessageId = message.MessageId,
                FromUserName = message.FromUsername,
                Message = message.Message,
                Subject = message.Subject
            };
        }
    }
}