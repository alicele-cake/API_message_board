using Microsoft.EntityFrameworkCore;
using messageboardAPI.Models;
using messageboardAPI.Models;
//using System.Collections.Generic;

namespace messageboardAPI.Models.Data
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }
        public DbSet<Comment> Comments { get; set; }
    }
}
