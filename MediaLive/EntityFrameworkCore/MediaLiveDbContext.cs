using MediaLive.Entities;
using Microsoft.EntityFrameworkCore;

namespace MediaLive.EntityFrameworkCore
{
    public class MediaLiveDbContext: DbContext
    {
        public MediaLiveDbContext(DbContextOptions options): base(options) {}
        
        public MediaLiveDbContext() {}
        
        public DbSet<Picture> Pictures => Set<Picture>();
        public DbSet<Video> Videos => Set<Video>();
    }
}