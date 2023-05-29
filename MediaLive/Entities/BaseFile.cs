using System;

namespace MediaLive.Entities
{
    public class BaseFile
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string NormalizedName { get; set; } = "";
        public string Name { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted => DeletedAt != null;

        public long Offset { get; set; } = 0L;
    }
}