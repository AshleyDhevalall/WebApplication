using System.Threading.Tasks;

namespace TestCaseApp
{
  public interface IDevopsUtility
  {
    Task<Project> GetProjectsAsync();
    Task GetProjectDescriptorAsync(string projectId);
  }
}
