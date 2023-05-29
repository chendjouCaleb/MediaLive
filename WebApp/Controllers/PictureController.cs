using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Models;
using MediaLive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace WebApp.Controllers
{

    [ApiController]
    [Route("api/pictures")]
    public class PictureController : ControllerBase
    {
        
        private readonly ILogger<PictureController> _logger;
        private readonly PictureService _pictureService;

        public PictureController(
            ILogger<PictureController> logger, 
            PictureService pictureService)
        {
            _logger = logger;
            _pictureService = pictureService;
        }

        public async Task<IEnumerable<Picture>> List()
        {
            return await _pictureService.List();
        }


        [HttpGet("{id}")]
        public async Task<Picture> Get(string id)
        {
            return await _pictureService.GetImageInfo(id);
        }

        [HttpGet("download")]
        public async Task<FileResult> Download([FromQuery] string pictureId)
        {
            var picture = await _pictureService.GetImageInfo(pictureId);

            string path = $"E:/Lab/Drive/{picture.Name}";
            
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;
            var ext = Path.GetExtension(path);
            var mime = MimeHelper.GetMime(ext);

            //Response.Headers.Add("Content-Disposition", "attachment;filename=some.txt");
            Response.Headers.Add("Content-Disposition", $"inline;filename={picture.Name}");
            return File(memoryStream, mime);
        }


        [HttpPost]
        public async Task<OkObjectResult> Upload(IFormFile file)
        {
            AddPictureOptions options = new()
            {
                FileName = file.FileName
            };
            var image = await _pictureService.AddImageAsync(file.OpenReadStream(), options);

            return Ok(image);
        }
    }
}