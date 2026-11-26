namespace NotikaIdentityEmail.Models
{
    public class MessageDetailViewModel
    {
        public int MessageId { get; set; }
        public string SenderEmail { get; set; }
        public string ReceiverEmail { get; set; }
        public string Subject { get; set; }
        public DateTime SendDate { get; set; }
        public string MessageDetail { get; set; }
        public bool IsRead { get; set; }

        public string CategoryName { get; set; }
        public string SenderName { get; set; }
        public string SenderSurname { get; set; }
    }
}
