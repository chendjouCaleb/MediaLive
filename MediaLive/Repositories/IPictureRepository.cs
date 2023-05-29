using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;

namespace MediaLive.Repositories
{
    public interface IPictureRepository
    {
        ValueTask<Picture?> GetByIdAsync(string id);

        ValueTask<Picture?> GetByNameAsync(string name);

        Task SaveAsync(Picture picture);

        ValueTask<IEnumerable<Picture>> ListAsync();
    }
}