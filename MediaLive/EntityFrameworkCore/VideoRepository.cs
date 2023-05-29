using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediaLive.EntityFrameworkCore
{
    public class VideoEFCoreRepository<TContext>:IVideoRepository where TContext: MediaLiveDbContext
    {
        private TContext _dbContext;

        public VideoEFCoreRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async ValueTask<Video?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<Video>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async ValueTask<Video?> GetByNameAsync(string name)
        {
            string normalizedName = StringHelper.Normalize(name);
            return await _dbContext
                .Set<Video>()
                .FirstOrDefaultAsync(p => p.NormalizedName == normalizedName);
        }

        public async Task SaveAsync(Video Video)
        {
            if (Video == null)
            {
                throw new ArgumentNullException(nameof(Video));
            }

            await _dbContext.AddAsync(Video);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(Video video)
        {
            _dbContext.Update(video);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<Video>> ListAsync()
        {
            return await _dbContext.Set<Video>().ToListAsync();
        }
    }
}