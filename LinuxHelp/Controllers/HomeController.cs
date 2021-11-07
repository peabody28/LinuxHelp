using LinuxHelp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net;
using System.Text.RegularExpressions;

namespace LinuxHelp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        // Обработка отправки формы (POST-запрос)
        [HttpPost]
        public string Index(SearchCommand comm)
        {
            
            string html = "";
            using (WebClient client = new WebClient())
                html = client.DownloadString("http://man.he.net/?topic="+comm.Command+"&section=all");

            int ind = html.IndexOf("No matches for");
            if (ind != -1)
                return "not found";
                    

            // парсинг
            int start = html.IndexOf("<PRE>");
            int end = html.IndexOf("</PRE>", start);

            string term = html.Substring(start, end-start);

            // delete links
            Regex regex = new Regex(@"<A(.+)/A>");
            MatchCollection matches = regex.Matches(term);
            if (matches.Count > 0)
            {
                foreach (Match match in matches)
                    term = term.Replace(match.Value, "");
            }
            term = term.Replace("\n<STRONG></STRONG>\n\n", "");
            return term ;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
