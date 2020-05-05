using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleVersioning.Data.SQLServer;
using SimpleVersioning.Models;
using System.Diagnostics;

namespace SimpleVersioning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, SqlServerContext context)
        {
            _logger = logger;
            context.Configurations.Add(new Configuration() { Name = "abc", Value = "abc" });
        }

        public IActionResult Index()
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
