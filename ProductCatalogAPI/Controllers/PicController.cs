using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ProductCatalogAPI.Controllers
{
    [Route("api/[controller]")] //api/pic/id
    [ApiController]
    public class PicController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;

        public PicController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Route("{id}")] 
        [HttpGet]
        public IActionResult GetImage(int id)
        {
            var webRoot = _env.WebRootPath; //WebRootPath gets the location of wwwroot

            var path = Path.Combine($"{webRoot}/pics/", $"Ring{id}.jpg");

            var buffer = System.IO.File.ReadAllBytes(path); //will read entire content

            return File(buffer, "image/jpeg"); //return content as a file to client
        }
    }
}
