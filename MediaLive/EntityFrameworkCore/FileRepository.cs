using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Helpers;
using MediaLive.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MediaLive.EntityFrameworkCore
{
    public class FileEFCoreRepository<TContext>:IFileRepository where TContext: MediaLiveDbContext
    {
        private TContext _dbContext;

        public FileEFCoreRepository(TContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async ValueTask<BaseFile?> GetByIdAsync(string id)
        {
            return await _dbContext.Set<BaseFile>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async ValueTask<BaseFile?> GetByNameAsync(string name)
        {
            string normalizedName = StringHelper.Normalize(name);
            return await _dbContext
                .Set<BaseFile>()
                .FirstOrDefaultAsync(p => p.NormalizedName == normalizedName);
        }

        public async Task SaveAsync(BaseFile baseFile)
        {
            if (baseFile == null)
            {
                throw new ArgumentNullException(nameof(baseFile));
            }

            await _dbContext.AddAsync(baseFile);
            await _dbContext.SaveChangesAsync();
        }

        public async ValueTask<IEnumerable<BaseFile>> ListAsync()
        {
            return await _dbContext.Set<BaseFile>().ToListAsync();
        }
    }
}