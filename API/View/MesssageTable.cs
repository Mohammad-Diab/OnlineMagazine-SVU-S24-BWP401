namespace OnlineStore
{
    public class MessageTable
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }


        public MessageTable()
        {
            Name = "";
            Email = "";
            Message = "";
        }

        public MessageTable(string name, string email, string message)
        {
            Name = name;
            Email = email;
            Message = message;
        }
    }
}
