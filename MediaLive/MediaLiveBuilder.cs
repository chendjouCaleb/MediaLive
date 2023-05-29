using System;
using System.IO;
using MediaLive.EntityFrameworkCore;
using MediaLive.LocalDriveStores;
using MediaLive.Repositories;
using MediaLive.Stores;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.DependencyInjection;

namespace MediaLive
{
    public class MediaLiveBuilder
    {
        private readonly IServiceCollection _services;

        public MediaLiveBuilder(IServiceCollection services)
        {
            _services = services;
        }

        public MediaLiveBuilder AddEntityFrameworkStores<T>() where T:MediaLiveDbContext
        {
            _services.AddTransient<IPictureRepository, PictureEFCoreRepository<T>>();
            _services.AddTransient<IVideoRepository, VideoEFCoreRepository<T>>();
            _services.AddTransient<IFileRepository, FileEFCoreRepository<T>>();
            return this;
        }

        public MediaLiveBuilder AddLocalFileStores(Action<LocalFileStoreOptions> optionAction)
        {
            var options = new LocalFileStoreOptions();
            optionAction.Invoke(options);

            if (string.IsNullOrWhiteSpace(options.DirectoryPath))
            {
                throw new InvalidOperationException("Directory path should not be empty or null.");
            }

            if (!Path.Exists(options.DirectoryPath))
            {
                throw new InvalidOperationException($"The dir '{options.DirectoryPath}' does not exists.");
            }

            _services.AddSingleton(options);
            _services.AddTransient<IFileStore, LocalFileStore>();
            return this;
        }
    }
}