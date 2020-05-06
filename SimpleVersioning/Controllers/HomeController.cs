using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SimpleVersioning.Data;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SimpleVersioning.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IStorageRepository storageSystem)
        {
            _logger = logger;
            storageSystem.Add(new File()
            {
                CreationTime = DateTime.Now,
                FileType = "abc",
                Name = "abc",
                Hash = "abc",
                LastUpdatedTime = DateTime.Now,
                Path = "abc",
                Properties = new List<FileProperty>() { new FileProperty() { Name = "abc", Value = "abc" } },
                Version = "1.2"
            });

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
