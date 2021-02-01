using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using WebApplication.Clients;
using WebApplication.Controllers;
using WebApplication.Models;
using Xunit;

namespace WebApplication.Tests
{
  public class PostControllerTests
  {
    private PostClientService CreateMockPostClientService()
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
      return new PostClientService(httpClient);
    }

   /// [Fact]
    public void TestGetPostById()
    {
      var builder = new DbContextOptionsBuilder<PostContext>()
         .UseInMemoryDatabase(databaseName: "database_name");

      var context = new PostContext(builder.Options);

      var posts = Enumerable.Range(1, 10)
       .Select(i => new Post { id = i, userid = i, title = $"Sample{i}", body = "Wrox Press" });
      context.Posts.AddRange(posts);
      int changed = context.SaveChanges();

      string expectedTitle = "Sample2";
      var controller = new PostController(context, CreateMockPostClientService());
      Post result = controller.Get(2);
      Assert.Equal(expectedTitle, result.title);
    }

  //  [Fact]
    public void TestCreatePost()
    {
      var builder = new DbContextOptionsBuilder<PostContext>()
         .UseInMemoryDatabase(databaseName: "database_name1");

      var context = new PostContext(builder.Options);     
      var post = new Post { id = 1, userid = 1, title = $"Sample{1}", body = "Wrox Press" };

      var controller = new PostController(context, CreateMockPostClientService());
      controller.Post(post);
      Assert.Equal(1, context.Posts.Count());
    }

  //  [Fact]
    public void TestDeletePost()
    {
      var builder = new DbContextOptionsBuilder<PostContext>()
         .UseInMemoryDatabase(databaseName: "database_name2");

      var context = new PostContext(builder.Options);

      var posts = Enumerable.Range(1, 10)
       .Select(i => new Post { id = i, userid = i, title = $"Sample{i}", body = "Wrox Press" });
      context.Posts.AddRange(posts);
      int changed = context.SaveChanges();

      var controller = new PostController(context, CreateMockPostClientService());
      controller.Delete(1);
      Assert.Equal(9, context.Posts.Count());
    }    
  }
}
