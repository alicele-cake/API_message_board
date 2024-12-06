using Microsoft.EntityFrameworkCore;
using messageboradAPI.Models;
//using System.Collections.Generic;

namespace messageboradAPI.Models.Data
{
    public class CommentContext : DbContext
    {
        public CommentContext(DbContextOptions<CommentContext> options) : base(options) { }
        public DbSet<Comment> Comments { get; set; }
    }
}
