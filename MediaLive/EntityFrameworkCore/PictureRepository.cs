using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediaLive.EntityFrameworkCore
{
    public class PictureEFCoreRepository<TContext>:IPictureRepository where TContext: MediaLiveDbContext
    {
        private TContext _dbContext;

        public PictureEFCoreRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async ValueTask<Picture?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<Picture>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async ValueTask<Picture?> GetByNameAsync(string name)
        {
            string normalizedName = StringHelper.Normalize(name);
            return await _dbContext
                .Set<Picture>()
                .FirstOrDefaultAsync(p => p.NormalizedName == normalizedName);
        }

        public async Task SaveAsync(Picture picture)
        {
            if (picture == null)
            {
                throw new ArgumentNullException(nameof(picture));
            }

            await _dbContext.AddAsync(picture);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<Picture>> ListAsync()
        {
            return await _dbContext.Set<Picture>().ToListAsync();
        }
    }
}