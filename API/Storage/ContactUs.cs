using System.Data.SqlClient;

namespace OnlineStore
{
    public class ContactUs
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string SentAt { get; set; }
        public string? ReadAt { get; set; }
        public string? DeletedAt { get; set; }


        static string BaseQuery = "SELECT ID, NAME, EMAIL, MESSAGE, SENT_AT, READ_AT, DELETED_AT FROM TBL_CONTACT_US ";

        public ContactUs()
        {
            Id = "";
            Name = "";
            Email = "";
            Message = "";
            SentAt = "";
        }

        public ContactUs(SqlDataReader reader)
        {
            Id = reader[reader.GetOrdinal("ID")].ToString() ?? "";
            Name = reader[reader.GetOrdinal("NAME")].ToString() ?? "";
            Email = reader[reader.GetOrdinal("EMAIL")].ToString() ?? "";
            Message = reader[reader.GetOrdinal("MESSAGE")].ToString() ?? "";
            SentAt = reader[reader.GetOrdinal("SENT_AT")].ToString() ?? "";
            ReadAt = reader[reader.GetOrdinal("READ_AT")].ToString() ?? null;
            DeletedAt = reader[reader.GetOrdinal("DELETED_AT")].ToString() ?? null;
        }

        public static List<ContactUs> GetMessagesList()
        {
            try
            {
                var contactUs = new List<ContactUs>();
                var whereString = " WHERE DELETED_AT IS NULL ";
                SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString);
                while (reader?.Read() ?? false)
                {
                    ContactUs contact = new ContactUs(reader);
                    contactUs.Add(contact);
                }
                reader?.Close();
                return contactUs;
            }
            catch (Exception) { throw; }
        }

        public static ContactUs? GetById(string id)
        {
            string whereString = $" WHERE ID = @0 ";
            SqlDataReader? reader = DatabaseConnection.Query(BaseQuery + whereString, id);
            if (reader?.Read() ?? false)
            {
                ContactUs contactUs = new ContactUs(reader);
                reader.Close();
                return contactUs;
            }
            return null;
        }

        public ContactUs(string name, string email, string message, string id = "")
        {
            Id = string.IsNullOrEmpty(id) ? Guid.NewGuid().ToString() : id;
            Name = name;
            Email = email;
            Message = message;
            SentAt = Helper.DateTimeToChar14(DateTime.UtcNow);
        }

        internal int MarkAsRead()
        {
            ReadAt = Helper.DateTimeToChar14(DateTime.UtcNow);
            string query = "UPDATE TBL_CONTACT_US SET READ_AT = @0 WHERE ID = @1";
            return DatabaseConnection.NonQuery(query, ReadAt, Id);
        }

        internal int Delete()
        {
            string query = "UPDATE TBL_CONTACT_US Set DELETED_AT = @0 WHERE ID = @1";
            return DatabaseConnection.NonQuery(query, Helper.DateTimeToChar14(DateTime.UtcNow), Id);
        }

        internal int Insert()
        {
            string query = "Insert into TBL_CONTACT_US (ID, NAME, EMAIL, MESSAGE, SENT_AT) VALUES (@0, @1, @2, @3, @4)";
            int result = DatabaseConnection.NonQuery(query, Id, Name, Email, Message, SentAt);
            return result;
        }
    }
}
