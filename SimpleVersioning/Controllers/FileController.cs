using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SimpleVersioning.Data;
using SimpleVersioning.Models;

namespace SimpleVersioning.Controllers
{
    [Route("/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        IStorageRepository repository;
        public FilesController(IStorageRepository repository)
        {
            this.repository = repository;
        }

        [HttpGet("/{name}/latest")]
        public File Get(string name)
        {
            return new File() { Id = 2 };
        }

        [HttpGet("/{name}/{version}")]
        public string Get(string name, string version)
        {
            return "value";
        }


    }
}
