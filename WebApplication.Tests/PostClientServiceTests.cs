﻿using Moq;
using Moq.Protected;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Clients;
using Xunit;

namespace WebApplication.Tests
{
  public class PostClientServiceTests
  {
    private System.Uri testUri = new System.Uri("https://jsonplaceholder.typicode.com/posts");
    private string testJson;

    [Fact]
    public async void ShouldReturnPosts()
    {
      var handlerMock = new Mock<HttpMessageHandler>();
      var response = new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(@"[{ ""id"": 1, ""title"": ""Cool post!""}, { ""id"": 100, ""title"": ""Some title""}]"),
      };

      handlerMock
         .Protected()
         .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
         .ReturnsAsync(response);

      var httpClient = new HttpClient(handlerMock.Object);
      var posts = new PostClientService(httpClient);

      var retrievedPosts = await posts.GetPosts();

      Assert.NotNull(retrievedPosts);
      handlerMock.Protected().Verify(
         "SendAsync",
         Times.Exactly(1),
         ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get),
         ItExpr.IsAny<CancellationToken>());
    }

    [Fact]
    public async void ShouldCallCreatePostApi()
    {
      var handlerMock = new Mock<HttpMessageHandler>();
      var response = new HttpResponseMessage
      {
        StatusCode = HttpStatusCode.OK,
        Content = new StringContent(@"{ ""id"": 101 }"),
      };
      handlerMock
         .Protected()
         .Setup<Task<HttpResponseMessage>>(
            "SendAsync",
            ItExpr.IsAny<HttpRequestMessage>(),
            ItExpr.IsAny<CancellationToken>())
         .ReturnsAsync(response);
      var httpClient = new HttpClient(handlerMock.Object);
      var posts = new PostClientService(httpClient);

      var retrievedPosts = await posts.CreatePost("Best post");

      handlerMock.Protected().Verify(
         "SendAsync",
         Times.Exactly(1),
         ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
         ItExpr.IsAny<CancellationToken>());
    }
  }
}
