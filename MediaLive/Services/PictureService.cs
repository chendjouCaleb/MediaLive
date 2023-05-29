using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Models;
using MediaLive.Repositories;
using MediaLive.Stores;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace MediaLive.Services
{
    public class PictureService
    {
        private readonly IPictureRepository _pictureRepository;
        private readonly IFileStore _fileStore;
        private readonly MediaLiveOptions _options;
        private readonly ILogger<PictureService> _logger;

        public PictureService(ILogger<PictureService> logger, 
            IPictureRepository pictureRepository, 
            IFileStore fileStore, 
            MediaLiveOptions options)
        {
            _logger = logger;
            _pictureRepository = pictureRepository;
            _fileStore = fileStore;
            _options = options;
        }


        public async Task<IEnumerable<Picture>> List()
        {
            return await _pictureRepository.ListAsync();
        }

        public async Task<Picture> GetImageInfo(string id)
        {
            var image = await _pictureRepository.GetByIdAsync(id);

            if (image == null)
            {
                
            }

            return image!;
        }

        public async Task<Picture> AddImageAsync(Stream stream, 
            AddPictureOptions options)
        {
            string normalizedFileName = StringHelper.Normalize(options.FileName);
            var picture = new Picture
            {
                NormalizedName = normalizedFileName,
                Name = options.FileName
            };

            await _pictureRepository.SaveAsync(picture);

            _fileStore.WriteFileAsync(stream, options.FileName);
            
            _logger.LogInformation("New image uploaded");

            return picture;
        }

        public async Task Resize(Image image, ResizePictureOptions options)
        {
            var thumbImage = new MemoryStream();
            image.Mutate(x => x.Resize( 100, 100));
            await image.SaveAsWebpAsync(thumbImage);
        }
    }
}