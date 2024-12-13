using messageboardAPI.Models;
using System.Data.Common;

namespace messageboardAPI.Models
{
    public static class DataReaderExtensions2
    {
            public static async Task<List<Comment>> MapToCommentsAsync(DbDataReader reader)
            {
                var comments = new List<Comment>();

                while (await reader.ReadAsync())
                {
                    comments.Add(new Comment
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("Id")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Message = reader.GetString(reader.GetOrdinal("Message")),
                        CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt"))
                    });
                }

                return comments;
            }
        


    }
}
