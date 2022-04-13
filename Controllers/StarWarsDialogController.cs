using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;


namespace StarWarsDialog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StarWarsDialogController : ControllerBase
    {

        private readonly IWebHostEnvironment _hostingEnvironment;
        //public StarWarsDialogController(IWebHostEnvironment hostingEnvironment)
        //{
        //    _hostingEnvironment = hostingEnvironment;
        //}

        //private static readonly string[] Summaries = new[]
        //{
        //    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        //};

        private readonly ILogger<StarWarsDialogController> _logger;
        private readonly IMemoryCache _memoryCache;

        private string[] LinesofDialog
        {
            get
            {
                string[] lines;
                if (!_memoryCache.TryGetValue("Star Wars", out lines))
                {
                    var path = System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, "data", "StarWars_IV.txt");
                    lines = System.IO.File.ReadAllLines(path);
                    _memoryCache.Set("Star Wars", lines);
                }
                return lines;
            }
        }
        public StarWarsDialogController(ILogger<StarWarsDialogController> logger, IWebHostEnvironment hostingEnvironment, IMemoryCache memoryCache)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
        }

        [HttpGet("line/{lineNumber}")]
        public string Hello(int lineNumber)
        {
            if (lineNumber > 0 && lineNumber <= LinesofDialog.Length)
            {
                return LinesofDialog[lineNumber - 1];
            }
            else
            {
                return "You must pass an integer between 1 and " + LinesofDialog.Length.ToString();
            }
        }

        [HttpGet]
        public string Get()
        {
            return "Usage: /lines will return all lines. /line/<linenumber> will return that line from the script (1-based).";
        }

        [HttpGet("lines")]
        public IEnumerable<string> Lines()
        {
            return LinesofDialog;
        }
    }
}
