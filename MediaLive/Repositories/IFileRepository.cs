using System.Collections.Generic;
using System.Threading.Tasks;
using MediaLive.Entities;

namespace MediaLive.Repositories
{
    public interface IFileRepository
    {
        ValueTask<BaseFile?> GetByIdAsync(string id);

        ValueTask<BaseFile?> GetByNameAsync(string name);

        Task SaveAsync(BaseFile baseFile);

        ValueTask<IEnumerable<BaseFile>> ListAsync();
    }
}