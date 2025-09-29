using HackerNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Text.Json;

namespace HackerNewsAPI.Services
{
    public class HackerNewsService : IHackerNewsService
    {
        private readonly HttpClient _httpClient;
        private readonly IMemoryCache _cache;

        public HackerNewsService(HttpClient httpClient, IMemoryCache cache)
        {
            _httpClient = httpClient;
            _cache = cache;
        }

        public async Task<List<StoryDto>> GetNewestStoriesAsync(int page, int pageSize)
        {
            // 1. Get cached story IDs (list of newest story IDs)
            const string idsCacheKey = "newestStories1";
            if (!_cache.TryGetValue(idsCacheKey, out List<int> storyIds))
            {
                var idsResponse = await _httpClient.GetStringAsync("https://hacker-news.firebaseio.com/v0/newstories.json");
                storyIds = JsonSerializer.Deserialize<List<int>>(idsResponse);

                // cache the list of IDs for 5 minutes
                _cache.Set(idsCacheKey, storyIds, TimeSpan.FromMinutes(5));
            }

            var pagedIds = storyIds.Skip((page - 1) * pageSize).Take(pageSize);

            // 3. Build story list with per-story caching
            var stories = new List<StoryDto>();
            foreach (var id in pagedIds)
            {
                var storyCacheKey = $"story_{id}";

                if (!_cache.TryGetValue(storyCacheKey, out StoryDto story))
                {
                    var storyJson = await _httpClient.GetStringAsync(
                        $"https://hacker-news.firebaseio.com/v0/item/{id}.json"
                    );
                    story = JsonSerializer.Deserialize<StoryDto>(storyJson);

                    if (story != null)
                    {
                        // cache each story for 5 minutes
                        _cache.Set(storyCacheKey, story, TimeSpan.FromMinutes(5));
                    }
                }

                if (story != null)
                    stories.Add(story);
            }

            return stories;
        }


    }
}
