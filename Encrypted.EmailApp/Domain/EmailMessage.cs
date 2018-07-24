namespace Encrypted.EmailApp.Domain
{
    public class EmailMessage
    {
        public int UserId { get; set; }
        public int MessageId { get; set; }
        public string FromUsername { get; set; }
        public string Subject{ get; set; }
        public string Message{ get; set; }
    }
}