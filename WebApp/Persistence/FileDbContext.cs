using MediaLive.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApp.Persistence
{
    public class FileDbContext:MediaLiveDbContext
    {
        public FileDbContext(DbContextOptions options) : base(options)
        {
        }

        public FileDbContext()
        {
        }
    }
}