using Aspose.Cells;
using ChoETL;
using GroupDocs.Conversion;
using GroupDocs.Conversion.Options.Load;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Parser.Models;
using System.Diagnostics;
using System.Text;
using System.Text.Json;

namespace Parser.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IWebHostEnvironment hostEnv;
       
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            hostEnv= env;
        }

        public IActionResult Index()
        {
            return View();
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