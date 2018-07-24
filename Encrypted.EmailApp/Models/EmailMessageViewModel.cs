using System.ComponentModel.DataAnnotations;

namespace Encrypted.EmailApp.Models
{
    public class EmailMessageViewModel
    {
        public int? MessageId { get; set; }

        [Required]
        public string ToUserName { get; set; }

        public string FromUserName { get; set; }

        public string Subject { get; set; }

        [Required]
        [MinLength(8)]
        public string EncryptionKey { get; set; }

        [Required]
        [MinLength(1)]
        public string Message { get; set; }
    }
}