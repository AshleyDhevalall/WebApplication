using Moq;
using Moq.Protected;
using NUnit.Framework;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using TestCaseApp;

namespace TestCaseApp.Client.Tests
{
  public class DevopsUtilityTests
  {
    private IDevopsUtility _postUtility;
    private Mock<HttpMessageHandler> _mockHttpMessageHandler;

    [SetUp]
    public async Task Setup()
    {
      _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
      _postUtility = new DevopsUtility(new HttpClient(_mockHttpMessageHandler.Object));
    }

    [Test]
    public async Task GetProjectsData()
    {
      // Arrange          
      _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
          ItExpr.IsAny<HttpRequestMessage>(),
          ItExpr.IsAny<CancellationToken>())
          .Returns(Task.FromResult(new HttpResponseMessage()
          {
            StatusCode = System.Net.HttpStatusCode.OK,
            Content = new StringContent(PostString())
          }));

      // Act
      var _result = await _postUtility.GetProjectsAsync();

      // Assert
      Assert.AreEqual(1, _result.count);
      Assert.AreEqual("One-POC", _result.value[0].name);
      Assert.AreEqual("edac31cb-c3d1-4bc7-b5a4-19902672d5c5", _result.value[0].id);
    }

    private string PostString()
    {
      return @"{""count"":1,""value"":[{""id"":""edac31cb-c3d1-4bc7-b5a4-19902672d5c5"",""name"":""One-POC"",""url"":""http://desktop-l66a62u:84/DefaultCollection/_apis/projects/edac31cb-c3d1-4bc7-b5a4-19902672d5c5"",""state"":""wellFormed"",""revision"":8,""visibility"":""private"",""lastUpdateTime"":""2020-11-20T23:13:20.473Z""}]}";
    }
  }
}
