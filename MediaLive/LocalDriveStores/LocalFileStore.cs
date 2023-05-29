using System;
using System.IO;
using System.Threading.Tasks;
using MediaLive.Entities;
using MediaLive.Stores;

namespace MediaLive.LocalDriveStores
{
    public class LocalFileStore:IFileStore
    {
        private LocalFileStoreOptions _options;

        public LocalFileStore(LocalFileStoreOptions options)
        {
            _options = options;
        }

        public async Task WriteFileAsync(Stream stream, string fileName)
        {
            string folderPath = _options.DirectoryPath;
            string filePath = Path.Join(folderPath, fileName);

            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await stream.CopyToAsync(fileStream);
        }

        public Task DeleteAsync(BaseFile file)
        {
            throw new System.NotImplementedException();
        }

        public async Task<FileStream> OpenWriteStream(BaseFile file)
        {
            string folderPath = _options.DirectoryPath;
            string filePath = Path.Join(folderPath, file.Name);

            Console.WriteLine("File path : " + filePath);

            var stream = new FileStream(filePath, FileMode.Append);
            Console.WriteLine("FileName: " + stream.Name);
            return stream;
        }
    }
}