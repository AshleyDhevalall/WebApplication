using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using WebApplication.Clients;
using WebApplication.Models;

namespace WebApplication.Controllers
{
  [ApiController]
  [Route("[controller]")] 
  public class PostController : ControllerBase
  {
    private readonly PostContext _postsContext;
    private readonly PostClientService _postClientService;

    public PostController(PostContext postsContext, 
      PostClientService postClientService)
    {
      _postsContext = postsContext;
      _postClientService = postClientService;
    }

    // GET: api/values
    [HttpGet]
    public IEnumerable<Post> Get() => _postsContext.Posts;

    // GET api/values/5
    [HttpGet("{id}")]
    public Post Get(int id) => _postsContext.Posts.Find(id);

    // POST api/values
    [HttpPost]
    public void Post([FromBody] Post value)
    {
      _postsContext.Posts.Add(value);
      _postsContext.SaveChanges();
    }

    // PUT api/values/5
    [HttpPut("{id}")]
    public void Put(int id, [FromBody] Post value)
    {
      if (value is null)
        throw new ArgumentNullException(nameof(value));
      if (id != value.id)
        throw new ArgumentException("id != value.id");
      _postsContext.Posts.Update(value);
      _postsContext.SaveChanges();
    }

    // DELETE api/values/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
      Post b = _postsContext.Posts.Find(id);
      _postsContext.Posts.Remove(b);
      _postsContext.SaveChanges();
    }
  }
}
