using HackerNewsAPI.Services;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;

namespace HackerNewsApi.Tests;

[TestFixture]

public class HackerNewsServiceTests
{

    private IMemoryCache _memoryCache;
    private HackerNewsAPI.Services.HackerNewsService _service;


    [SetUp]
    public void Setup()
    {
        _memoryCache = new MemoryCache(new MemoryCacheOptions());
        _service = new HackerNewsService(new HttpClient(), _memoryCache);
    }

    [TearDown]
    public void TearDown()
    {
        _memoryCache.Dispose();
    }

    [Test]
    public async Task GetNewestStoriesAsync_ReturnsStories()
    {
        var result = await _service.GetNewestStoriesAsync(1, 5);
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count, Is.LessThanOrEqualTo(5));

    }


}
