using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;

namespace MediaLive.Repositories
{
    public interface IVideoRepository
    {
        ValueTask<Video?> GetByIdAsync(string id);

        ValueTask<Video?> GetByNameAsync(string name);

        Task SaveAsync(Video video);

        Task UpdateAsync(Video video);

        ValueTask<IEnumerable<Video>> ListAsync();
    }
}