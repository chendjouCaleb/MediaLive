using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Models;
using MediaLive.Repositories;
using MediaLive.Stores;
using Microsoft.Extensions.Logging;

namespace MediaLive.Services
{
    public class VideoService
    {
        
        private readonly IVideoRepository _videoRepository;
        private readonly IFileStore _fileStore;
        private readonly MediaLiveOptions _options;
        private readonly ILogger<VideoService> _logger;

        public VideoService(ILogger<VideoService> logger, 
            IVideoRepository videoRepository, 
            IFileStore fileStore, 
            MediaLiveOptions options)
        {
            _logger = logger;
            _videoRepository = videoRepository;
            _fileStore = fileStore;
            _options = options;
        }
        
        
        public async Task<IEnumerable<Video>> List()
        {
            return await _videoRepository.ListAsync();
        }

        public async Task<Video> GetImageInfo(string id)
        {
            var image = await _videoRepository.GetByIdAsync(id);

            if (image == null)
            {
                
            }

            return image!;
        }

        public async Task<Video> AddImageAsync(Stream stream,
            AddVideoOptions options)
        {
            string normalizedFileName = StringHelper.Normalize(options.FileName);
            var Video = new Video
            {
                NormalizedName = normalizedFileName,
                Name = options.FileName
            };

            await _videoRepository.SaveAsync(Video);

            _fileStore.WriteFileAsync(stream, options.FileName);

            _logger.LogInformation("New image uploaded");

            return Video;
        }
        
        
        public async Task<Video> AddImageAsync(AddVideoOptions options)
        {
            string normalizedFileName = StringHelper.Normalize(options.FileName);
            var Video = new Video
            {
                NormalizedName = normalizedFileName,
                Name = options.FileName
            };
            await _videoRepository.SaveAsync(Video);
            return Video;
        }

        public async Task<FileStream> GetWriteStream(Video video)
        {
            return await _fileStore.OpenWriteStream(video);
        }
    }
}