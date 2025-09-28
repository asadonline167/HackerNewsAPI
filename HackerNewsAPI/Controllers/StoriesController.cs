using HackerNewsAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace HackerNewsAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class StoriesController : Controller
    {

        private readonly IHackerNewsService _service;

        public StoriesController(IHackerNewsService service)
        {
            _service = service;
        }

        [HttpGet("newest")]
        public async Task<IActionResult> GetNewestStories(int page = 1, int pageSize = 20)
        {
            var stories = await _service.GetNewestStoriesAsync(page, pageSize);
            return Ok(stories);
        }

    }
}
