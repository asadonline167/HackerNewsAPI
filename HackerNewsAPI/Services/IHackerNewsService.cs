using HackerNewsAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Services
{
    public interface IHackerNewsService
    {
        Task<List<StoryDto>> GetNewestStoriesAsync(int page, int pageSize);
    }

}
