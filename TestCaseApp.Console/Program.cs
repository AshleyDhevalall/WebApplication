using System.Threading.Tasks;

namespace TestCaseApp.Console
{
  class Program
  {
    static async Task Main(string[] args)
    {
      IDevopsUtility post = new DevopsUtility();

      var projects = await post.GetProjectsAsync();
      System.Console.WriteLine($" Id: {projects.value[0].id}");

      await post.GetProjectDescriptorAsync(projects.value[0].id);


      System.Console.ReadLine();
    }
  }
}
