using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace APITests;

public class TestGetMoviesRequests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly Uri _baseAddress = new("http://localhost:5141/api/movies");
    private readonly HttpClient _client;
    private long[] Ids { get;  } = { 1, 2, 3, 4 };

    public TestGetMoviesRequests(WebApplicationFactory<Program> application)
    {
        _client = application.CreateClient();
    }

    private static void GET_IdSpecifiedMovieStatusCode(HttpStatusCode responseStatusCode, long id)
    {
        Assert.Equal(HttpStatusCode.OK, responseStatusCode);
    }

    private static void GET_IdSpecifiedMovieResult(string responseContent, long id)
    {
        Assert.Equal(responseContent, new MovieListModel().GetIdSpecifiedSeedMovieInJsonFormat(id));
    }

    [Fact]
    public async Task GET_ListOfMovies()
    {
        var response = await _client.GetAsync(_baseAddress);
        var responseContent = await response.Content.ReadAsStringAsync();
        Assert.Equal(responseContent, new MovieListModel().GetSeedMovieListInJsonFormat());
    }
    
    [Fact]
    //RootEndPointStatusCode
    public async Task GET_ListOfMoviesStatusCode()
    {
        var response = await _client.GetAsync(_baseAddress);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
    
    [Fact]
    public async Task GET_IdSpecifiedMovieResultAndStatusCode()
    {
        foreach (var id in Ids)
        {
            var response = await _client.GetAsync($"{_baseAddress}/{id}");
            var responseContent = await response.Content.ReadAsStringAsync();
            GET_IdSpecifiedMovieStatusCode(response.StatusCode, id);
            GET_IdSpecifiedMovieResult(responseContent, id);
        }
    }
}
