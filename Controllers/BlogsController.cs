using System.Collections.Generic;
using System.Threading.Tasks;
using bloggr_csharp.Models;
using bloggr_csharp.Services;
using CodeWorks.Auth0Provider;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace bloggr_csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BlogsController : ControllerBase
    {
        private readonly BlogsService _bservice;
        private readonly CommentsService _cservice;

        public BlogsController(BlogsService bservice, CommentsService cservice)
        {
            _bservice = bservice;
            _cservice = cservice;
        }

        [HttpGet]
        public ActionResult<Blog> Get()
        {
            try
            {
                return Ok(_bservice.GetAll());
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}")]
        public ActionResult<Blog> Get(int id)
        {
            try
            {
                return Ok(_bservice.GetById(id));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Blog>> CreateAsync([FromBody] Blog newBlog)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newBlog.CreatorId = userInfo.Id;
                Blog created = _bservice.Create(newBlog);

                return Ok(created);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Blog>> EditAsync([FromBody] Blog editData, int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editData.Id = id;
                editData.CreatorId = userInfo.Id;
                Blog editedBlog = _bservice.Edit(editData);
                return Ok(editedBlog);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Blog>> DeleteAsync(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_bservice.Delete(id, userInfo.Id));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpGet("{id}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments(int id)
        {
            try
            {
                return Ok(_cservice.GetCommentsByBlogId(id));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}