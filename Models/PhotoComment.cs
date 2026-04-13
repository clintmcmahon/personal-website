using System;

namespace Website.Models
{
    public class PhotoComment
    {
        public int Id { get; set; }
        public string PhotoDate { get; set; } // e.g., "2026-04-10"
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
