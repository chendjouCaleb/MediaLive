using System;

namespace MediaLive.Entities
{
    public class Upload
    {
        public string Id { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        
    }
}