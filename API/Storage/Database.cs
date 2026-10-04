using System;
using System.Data;
using System.Data.SqlClient;

namespace OnlineStore
{
    internal static class DatabaseConnection
    {
        static SqlConnection? sqlConnection;

        internal static void SetConnectionStringValue(string? connectionString, string serverUrl, string databaseName, string username, string password)
        {
            ConnectionString = string.IsNullOrEmpty(connectionString) 
                ? $"Data Source={serverUrl};Initial Catalog={databaseName};User ID={username};Password={password};Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"
                : connectionString;
        }

        static string? ConnectionString { get; set; }

        internal static SqlDataReader? Query(string SQLStatment, params string[] Parameters)
        {
            if ((sqlConnection?.State ?? ConnectionState.Closed) != ConnectionState.Closed)
            {
                try
                {
                    sqlConnection?.Close();
                }
                catch (Exception){ }
            }

            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(SQLStatment, sqlConnection);

                for (int i = 0; i < Parameters.Length; i++)
                {
                    command.Parameters.AddWithValue("@" + i, Parameters[i]);
                }
                SqlDataReader result = command.ExecuteReader();
                return result;

            }
            catch (Exception)
            {
                return null;
            }
        }

        internal static int NonQuery(string SQLStatment, params object[] Parameters)
        {
            if ((sqlConnection?.State ?? ConnectionState.Closed) != ConnectionState.Closed)
            {
                try
                {
                    sqlConnection?.Close();
                }
                catch (Exception){}
            }
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(SQLStatment, sqlConnection);
                for (int i = 0; i < Parameters.Length; i++)
                {
                    command.Parameters.AddWithValue("@" + i, Parameters[i]);
                }
                int result = command.ExecuteNonQuery();

                return result;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        internal static string Scalar(string SQLStatment, params string[] Parameters)
        {
            if ((sqlConnection?.State ?? ConnectionState.Closed) != ConnectionState.Closed)
            {
                try
                {
                    sqlConnection?.Close();
                }
                catch (Exception) { }
            }
            try
            {
                sqlConnection = new SqlConnection(ConnectionString);
                sqlConnection.Open();
                SqlCommand command = new SqlCommand(SQLStatment, sqlConnection);
                for (int i = 0; i < Parameters.Length; i++)
                {
                    command.Parameters.AddWithValue("@" + i, Parameters[i]);
                }

                string result = command.ExecuteScalar().ToString() ?? "";
                return result;

            }
            catch (Exception)
            {
                return "";
            }
        }

        internal static bool CheckVariable(string code)
        {
            return true;
        }

        internal static int ConvertToInt32(object v)
        {
            if (v is DBNull)
                return 0;
            else
            {
                int result;
                int.TryParse(v.ToString(), out result);
                return result;
            }
        }

        internal static long ConvertToInt64(object v)
        {
            if (v is DBNull)
                return 0;
            else
            {
                long result;
                long.TryParse(v.ToString(), out result);
                return result;
            }
        }

        internal static bool ConvertToBoolean(object v)
        {
            if (v is DBNull)
                return false;
            else
            {
                bool result;
                bool.TryParse(v.ToString(), out result);
                return result;
            }
        }
    }

}
