using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Models
{
    public class StoryDto
    {
        public int id { get; set; }
        public string title { get; set; }
        public string url { get; set; }

    }
}