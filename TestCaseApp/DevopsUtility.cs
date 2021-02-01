using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace TestCaseApp
{
  public class DevopsUtility : IDevopsUtility
  {
    HttpClient _client;
    string baseUrl = "http://desktop-l66a62u:84/DefaultCollection";
    string personalaccesstoken = "3dhr73udewnibz7aeixhjndk7jdgzmqrc3mvvbtr2zcdzswzinmq";

    public DevopsUtility()
    {

    }

    public DevopsUtility(HttpClient client)
    {
      _client = client;
    }

    public async Task<Project> GetProjectsAsync()
    {      
      using (var client = _client ?? new HttpClient())
      {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
               Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", "", personalaccesstoken))));

        HttpResponseMessage response = await client.GetAsync($"{baseUrl}/_apis/projects?api-version=5.0");
        response.EnsureSuccessStatusCode();

        using (HttpContent content = response.Content)
        {
          string responseBody = await response.Content.ReadAsStringAsync();
          var projects = JsonConvert.DeserializeObject<Project>(responseBody);
          return projects;
        }
      }
    }

    public async Task GetProjectDescriptorAsync(string projectId)
    {
      using (var client = _client ?? new HttpClient())
      {
        client.DefaultRequestHeaders.Accept.Clear();
        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
               Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes(
                       string.Format("{0}:{1}", "", personalaccesstoken))));

        HttpResponseMessage response = await client.GetAsync($"https://vssps.dev.azure.com/DefaultCollection/_apis/graph/descriptors/{projectId}");
        response.EnsureSuccessStatusCode();

        using (HttpContent content = response.Content)
        {
          string responseBody = await response.Content.ReadAsStringAsync();
          var projects = JsonConvert.DeserializeObject<Project>(responseBody);
         // return projects;
        }
      }
    }
   // https://vssps.dev.azure.com/{my_org}/_apis/graph/descriptors/{myProjectID}
  }
}
