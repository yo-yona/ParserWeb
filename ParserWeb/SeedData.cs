using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;


namespace ParserWeb.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Cash c1 = new Cash
            {
                Site = "vk.com",
                Date = DateTime.Parse("1984-3-13"),
            };

            Cash c2 = new Cash
            {
                Site = "Ghostbusters.ru",
                Date = DateTime.Parse("1959-4-15"),
            };
            using (var context = new ParserContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<ParserContext>>()))
            {
                //context.Database.EnsureDeleted();
                // Look for any movies.
                if (context.Cashs.Any())
                {
                    foreach (var c in context.Cashs.ToArray())
                    {
                        //Console.WriteLine($"{c.Id}, {c.Site}, {c.Date}");
                    }
                    Console.WriteLine(context.Cashs.Count());
                    return;   // DB has been seeded
                }

                context.Cashs.AddRange(
                    c1, c2
                );

                context.SiteContents.AddRange(
                    new SiteContent
                    {
                        Site = c1,
                        Word = "vk",
                        Count = 1
                    },
                    new SiteContent
                    {
                        Site = c1,
                        Word = "durov",
                        Count = 13
                    },

                    new SiteContent
                    {
                        Site = c2,
                        Word = "Ghost",
                        Count = 1
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
