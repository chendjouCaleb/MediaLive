using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Models;
using MediaLive.Repositories;
using MediaLive.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Filters;

namespace WebApp.Controllers
{

    [ApiController]
    [Route("api/videos")]
    public class VideoController : ControllerBase
    {
        
        private readonly ILogger<VideoController> _logger;
        private readonly VideoService _videoService;
        private readonly IVideoRepository _videoRepository;

        public VideoController(
            ILogger<VideoController> logger, 
            VideoService videoService, IVideoRepository videoRepository)
        {
            _logger = logger;
            _videoService = videoService;
            _videoRepository = videoRepository;
        }

        public async Task<IEnumerable<Video>> List()
        {
            return await _videoService.List();
        }


        [HttpGet("{id}")]
        public async Task<Video> Get(string id)
        {
            return await _videoService.GetImageInfo(id);
        }

        [HttpGet("download")]
        public async Task<FileResult> Download([FromQuery] string videoId)
        {
            var video = await _videoService.GetImageInfo(videoId);

            string path = $"E:/Lab/Drive/{video.Name}";
            
            var memoryStream = new MemoryStream();
            using (var stream = new FileStream(path, FileMode.Open))
            {
                await stream.CopyToAsync(memoryStream);
            }

            memoryStream.Position = 0;
            var ext = Path.GetExtension(path);
            var mime = MimeHelper.GetMime(ext);

            //Response.Headers.Add("Content-Disposition", "attachment;filename=some.txt");
            Response.Headers.Add("Content-Disposition", $"inline;filename={video.Name}");
            return File(memoryStream, mime);
        }


        [HttpPost]
        public async Task<OkObjectResult> Upload()
        {
            AddVideoOptions options = new()
            {
                FileName = $"{Guid.NewGuid()}.mp4"
            };
            var video = await _videoService.AddImageAsync(options);

            return Ok(video);
        }

        [HttpPost("append")]
        public async Task<OkResult> UploadChunk([FromQuery]string videoId,
            [FromQuery] int offset,
            IFormFile file)
        {
            Console.WriteLine("Open append write.");
            var video = await _videoRepository.GetByIdAsync(videoId)!;
            var writeStream = await _videoService.GetWriteStream(video!);

            var buffer = new byte[file.Length];
            await file.OpenReadStream().ReadAsync(buffer, 0, (int) file.Length);
            
            
            await writeStream.WriteAsync(buffer);
            writeStream.Close();

            video.Offset += file.Length;
            await _videoRepository.UpdateAsync(video);

            return Ok();
        } 
    }
}