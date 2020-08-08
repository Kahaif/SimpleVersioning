using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SimpleVersioning;
using SimpleVersioning.Models;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

namespace Testing
{
    [TestClass]
    public class FilesControllerTest
    {

        private readonly HttpClient client;
        private readonly TestServer server;
        public FilesControllerTest()
        {
            server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>()
                .UseSetting("ConnectionStrings:SimpleVersioning",
                            "Server=80.219.88.57;Port=3306;Database=SimpleVersioning;User=simple-versioning;Password=McdlMp$123;"));
            client = server.CreateClient();
        }
      

        [TestMethod]
        public async Task AssertAddFiles()
        {
            File file = new File();

            var response = await client.PostAsJsonAsync("/files", file);

            // No ressourcename, no name, no versions ...
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            file.ResourceName = "Not working ressource $$$";

            response = await client.PostAsJsonAsync("/files", file);
            Assert.IsTrue(response.StatusCode == HttpStatusCode.BadRequest);

            file.ResourceName = "working-ressource";
            file.Name = "Working ressource";
            file.Versions = new List<FileVersion>()
            {
                new FileVersion()
                {
                    Type = "xml",
                    Version = "1-1-1-1-1",
                    Path = "present",
                }
            };
            
            response = await client.PostAsJsonAsync("/files", file);
            var tg = await response.Content.ReadAsStringAsync();
            Assert.IsTrue(response.IsSuccessStatusCode);
        }

        [TestMethod]
        public async Task AssertAddVersions()
        {

            List<FileVersion> versions = new List<FileVersion>()
            {
                new FileVersion()
                {
                    Description = "...",
                    Path = "...",
                    CreationTime = DateTime.Now,
                    LastUpdatedTime = DateTime.Now,
                    Hash = "...",
                    Type = "..:",
                   Version = "..."
                }
            };
            File f = Helper.GetRandomFiles(1)[0];

            var response = await client.PostAsJsonAsync("/files", f);
            Assert.IsTrue(response.IsSuccessStatusCode);

            response = await client.PostAsJsonAsync($"/files/{f.ResourceName}", versions);
            Assert.IsTrue(response.IsSuccessStatusCode);
        }
    }
}
