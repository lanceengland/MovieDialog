using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;

namespace MovieDialog.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieDialogController : ControllerBase
    {
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly ILogger<MovieDialogController> _logger;
        private readonly IMemoryCache _memoryCache;
        private readonly IConfiguration _configuration;
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
        public MovieDialogController(
            ILogger<MovieDialogController> logger, 
            IWebHostEnvironment hostingEnvironment, 
            IMemoryCache memoryCache,
            IConfiguration configuration)
        {
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
            _memoryCache = memoryCache;
            _configuration = configuration;
        }

        private MovieDialog.Dialog[] GetFromCache(string movie)
        {
            try
            {
                IEnumerable<MovieDialog.Dialog> lines;

                if (!_memoryCache.TryGetValue("Star Wars", out lines))
                {
                    var section = this._configuration.GetSection("FileNames");
                    var fileName = section.GetValue<string>(movie);
                    var path = System.IO.Path.Combine(_hostingEnvironment.ContentRootPath, "data", fileName);

                    var dialogCollection = new System.Collections.Generic.List<MovieDialog.Dialog>();

                    foreach (var line in System.IO.File.ReadAllLines(path))
                    {
                        var d = new MovieDialog.Dialog();
                        var fields = line.Split("|");
                        d.Speaker = fields[1];
                        d.Line = fields[2];
                        dialogCollection.Add(d);
                    }

                    _memoryCache.Set(movie, dialogCollection.ToArray());
                }
                return _memoryCache.Get<MovieDialog.Dialog[]>(movie);
            }
            catch
            {
                return new MovieDialog.Dialog[0];
            }
        }

        [HttpGet("lines/{movie}")]
        public IEnumerable<MovieDialog.Dialog> Lines(string movie)
        {
            return this.GetFromCache(movie);
        }

        [HttpGet("line/{movie}/{number}")]
        public MovieDialog.Dialog Line(string movie, int number)
        {
            var dialog = this.GetFromCache(movie);
            if(number >= 0 && number < dialog.Length)
            {
                return dialog[number];
            }
            else
            {
                return new MovieDialog.Dialog();
            }
        }

        [HttpGet("random/{movie}")]
        public MovieDialog.Dialog Random(string movie)
        {
            var dialog = this.GetFromCache(movie);
            var rnd = new System.Random();
            var i = rnd.Next(0, dialog.Length);
            return dialog[i];
        }
    }
}
