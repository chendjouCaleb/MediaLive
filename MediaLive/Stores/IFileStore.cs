using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;

namespace MediaLive.Stores
{
    public interface IFileStore
    {
        public Task WriteFileAsync(Stream fileStream, string fileName);
        public Task DeleteAsync(BaseFile file);

        public Task<FileStream> OpenWriteStream(BaseFile file);
    }
}