using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppSoftwareExamen.Classes
{
    public class db
    {
        MySqlConnection connection;
        private string server, database, username, password;

        public db()
        {
            server = "146.59.153.218";
            database = "examenAPI";
            username = "examenAPI";
            password = "APIex123!";

            connection = new MySqlConnection($"server={server};port=3306;uid={username};pwd={password};database={database}");
        }

        public async Task InsertPingResultAsync(string sender, string receiver, string timeMs)
        {
            try
            {
                await connection.OpenAsync();

                string query = "INSERT INTO requests (sender, receiver, time_ms) VALUES (@sender, @receiver, @timeMs)";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                cmd.Parameters.AddWithValue("@sender", sender);
                cmd.Parameters.AddWithValue("@receiver", receiver);
                cmd.Parameters.AddWithValue("@timeMs", timeMs);

                await cmd.ExecuteNonQueryAsync();
            }
            finally
            {
                connection.Close();
            }
        }
        public async Task<List<PreviousPingResult>> GetPreviousPingsAsync()
        {

            List<PreviousPingResult> pingResults = new List<PreviousPingResult>();
            try
            {
                await connection.OpenAsync();

                string query = "SELECT sender, receiver, time_ms FROM requests";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                using (MySqlDataReader reader = (MySqlDataReader)await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        pingResults.Add(new PreviousPingResult
                        {
                            Sender = reader.GetString("sender"),
                            Receiver = reader.GetString("receiver"),
                            TimeMs = reader.GetString("time_ms")
                        });
                    }
                }
            }
            finally
            {
                connection.Close();
            }
            return pingResults;
        }
        public class PreviousPingResult
        {
            public string Sender { get; set; }
            public string Receiver { get; set; }
            public string TimeMs { get; set; }
        }

    }
}
