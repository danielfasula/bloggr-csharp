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
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _cservice;
        private readonly BlogsService _bservice;

        public CommentsController(CommentsService cservice, BlogsService bservice)
        {
            _cservice = cservice;
            _bservice = bservice;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Comment>> CreateAsync([FromBody] Comment newComment)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                newComment.CreatorId = userInfo.Id;
                Comment created = _cservice.Create(newComment);

                return Ok(created);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> EditAsync([FromBody] Comment editData, int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                editData.Id = id;
                editData.CreatorId = userInfo.Id;
                Comment editedComment = _cservice.Edit(editData);
                return Ok(editedComment);
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Comment>> DeleteAsync(int id)
        {
            try
            {
                Profile userInfo = await HttpContext.GetUserInfoAsync<Profile>();
                return Ok(_cservice.Delete(id, userInfo.Id));
            }
            catch (System.Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}