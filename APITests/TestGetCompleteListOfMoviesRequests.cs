using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace APITests;

public class TestGetMoviesRequests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Uri _baseAddress = new("http://localhost:7273/api/movies");
    private readonly HttpClient _client;

    public TestGetMoviesRequests(WebApplicationFactory<Program> application)
    {
        _client = application.CreateClient();
    }

    [Fact]
    private async Task GET_ListOfMovies()
    {
        var response = await _client.SendAsync(new HttpRequestMessage()
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"{_baseAddress}")
        });
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(responseContent, TestDataExpected.GetListOfMoviesExpectedResult);
    }
    
    [Fact]
    //RootEndPointStatusCode
    private async Task GET_ListOfMoviesStatusCode()
    {
        var response = await _client.GetAsync(_baseAddress);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
}
