using Microsoft.AspNetCore.Mvc;
using ParserWeb.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace ParserWeb.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ParserContext _context;

        public HomeController(ILogger<HomeController> logger, ParserContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View(_context.SiteContents.Include(s => s.Site).ToList());
        }
        //[HttpPost]
        public async Task<IActionResult> Result(string site)
        {
            Dictionary<string, uint> results = new Dictionary<string, uint>();
            /*if (String.IsNullOrWhiteSpace(site))
                site = "https://simbirsoft.com/";*/
            var fromDB = await _context.Cashs.FirstOrDefaultAsync(c => c.Site == site);
            if (fromDB == null && !string.IsNullOrEmpty(site))
            {
                WebAgent webConnection = new WebAgent(site);
                results = webConnection.PrintStatistics();
                Cash new_entry_cash = new Cash { Date = DateTime.Now, Site = site };
                _context.Cashs.Add(new_entry_cash);
                Console.WriteLine(_context.Cashs.ToArray()[0].Site);
                Console.WriteLine(_context.SiteContents.ToArray()[0].Word);
                Console.WriteLine(_context.SiteContents.ToArray()[0].Site.Site);
                foreach (var result in results)
                {
                    _context.SiteContents.Add(new SiteContent { Word = result.Key, Count = result.Value, Site = new_entry_cash });
                }
                _context.SaveChanges();
            }
                var counts = from c in _context.SiteContents
                             select c;
                counts = counts.Where(c => c.Site.Site.Contains(site));
            return View(counts.OrderByDescending(entry => entry.Count).Include(s => s.Site).ToList());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}