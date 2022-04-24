#nullable disable
using Microsoft.EntityFrameworkCore;

namespace ParserWeb.Models
{
    public class ParserContext : DbContext
    {
        public ParserContext(DbContextOptions<ParserContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<ParserWeb.Models.Cash> Cashs { get; set; }
        public DbSet<ParserWeb.Models.SiteContent> SiteContents { get; set; }
    }
}
