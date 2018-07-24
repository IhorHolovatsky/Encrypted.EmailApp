using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Encrypted.EmailApp.Domain;
using Encrypted.EmailApp.Repositories.Interfaces;
using Encrypted.EmailApp.Utils;
using MySql.Data.MySqlClient;

namespace Encrypted.EmailApp.Repositories
{
    public class EmailMessageRepository : IEmailMessageRepository
    {
        public Task<List<EmailMessage>> GetMessagesAsync()
        {
            return GetMessages();
        }

        public async Task<EmailMessage> GetMessageByIdAsync(int messageId)
        {
            return (await GetMessages(messageId)).FirstOrDefault();
        }

        public async Task<EmailMessage> SendMessageAsync(EmailMessage message)
        {
            using (var connection = new MySqlConnection(Configuration.MySqlConnectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "INSERT INTO `messages` " +
                                      "(`userid`, `fromusername`, `subject`, `message`) " +
                                      "VALUES (@userId, @fromusername, @subject, @message); " +
                                      "SELECT LAST_INSERT_ID();";

                command.Parameters.AddWithValue("@userId", message.UserId);
                command.Parameters.AddWithValue("@fromusername", message.FromUsername ?? "Ihor");
                command.Parameters.AddWithValue("@subject", message.Subject);
                command.Parameters.AddWithValue("@message", message.Message);

                var messageId = (ulong)command.ExecuteScalar();

                return (await GetMessages((int?)messageId)).FirstOrDefault();
            }
        }

        private async Task<List<EmailMessage>> GetMessages(int? messageId = null)
        {
            using (var connection = new MySqlConnection(Configuration.MySqlConnectionString))
            {
                await connection.OpenAsync();

                var command = connection.CreateCommand();
                command.CommandText = "SELECT  `messageId`,  `userid`,  `fromusername`,  `subject`,  `message` " +
                                      "FROM `messages` " +
                                      "WHERE 1 = 1 ";

                if (messageId.HasValue)
                {
                    command.CommandText += "AND @messageId = `messageId`";
                    command.Parameters.AddWithValue("@messageId", messageId.Value);
                }

                var messages = new List<EmailMessage>();
                var ds = new DataSet();
                using (var da = new MySqlDataAdapter(command))
                {
                    da.Fill(ds);
                }

                if (ds.Tables.Count == 0
                    || ds.Tables[0].Rows.Count == 0)
                    return messages;


                return ds.Tables[0].Rows.Cast<DataRow>()
                    .Select(dr => new EmailMessage
                    {
                        MessageId = dr.Field<int>("messageId"),
                        UserId = dr.Field<int>("userid"),
                        FromUsername = dr.Field<string>("fromusername"),
                        Subject = dr.Field<string>("subject"),
                        Message = dr.Field<string>("message"),
                    })
                    .ToList();
            }
        }
    }
}