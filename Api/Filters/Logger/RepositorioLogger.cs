using System.Collections.Generic;
using System.Data.SqlClient;
using Api;
using Microsoft.Extensions.Options;

namespace Application.Filters.Logger
{
    public class RepositorioLogger
    {
        private string ConnectionString { get; set; }
       

        public RepositorioLogger( string connection)
        {
            ConnectionString = connection;
            
        }

        private bool ExecuteNonQuery(string commandStr, List<SqlParameter> paramList)
        {
            var result = false;
            using (var conn = new SqlConnection(ConnectionString))
            {
                if (conn.State != System.Data.ConnectionState.Open)
                {
                    conn.Open();
                }

                using (var command = new SqlCommand(commandStr, conn))
                {
                    command.Parameters.AddRange(paramList.ToArray());
                    var count = command.ExecuteNonQuery();
                    result = count > 0;
                }
            }
            return result;
        }

        public bool InsertLog(ApplicationLog log)
        {
            var command = $@"INSERT INTO [dbo].[LogErroAPP] ([Application], [LogLevel] ,[Action], [Message],[EventDate]) VALUES (@Application, @LogLevel, @Action, @Message, @EventDate)";
            var paramList = new List<SqlParameter>
            {
                new SqlParameter("Application", log.Application),
                new SqlParameter("LogLevel", log.LogLevel),
                new SqlParameter("Action", log.Action),
                new SqlParameter("Message", log.Message),
                new SqlParameter("EventDate", log.EventDate)
            };

            return ExecuteNonQuery(command, paramList);
        }
    }
}
