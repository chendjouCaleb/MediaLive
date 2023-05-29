using System;
using MediaLive.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MediaLive
{
    public static class MediaLiveConfigure
    {
        public static MediaLiveBuilder AddMediaLive(this IServiceCollection services,
            Action<MediaLiveOptions> optionAction)
        {
            var options = new MediaLiveOptions();
            optionAction(options);
            
            var builder = new MediaLiveBuilder(services);
            services.AddTransient<PictureService>();
            services.AddTransient<VideoService>();
            services.AddSingleton(options);

            return builder;
        }
    }
}