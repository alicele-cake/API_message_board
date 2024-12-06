using System;
namespace messageboradAPI.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
