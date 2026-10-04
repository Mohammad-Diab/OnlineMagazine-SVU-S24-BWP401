namespace OnlineStore
{
    public class MessageGrid
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string SentAt { get; set; }
        public bool IsRead { get; set; }


        public MessageGrid(ContactUs message)
        {
            Id = message.Id;
            Name = message.Name;
            Email = message.Email;
            SentAt = message.SentAt;
            IsRead = !string.IsNullOrEmpty(message.ReadAt);
        }
    }
}
