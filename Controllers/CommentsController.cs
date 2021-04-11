using bloggr_csharp.Models;
using bloggr_csharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace bloggr_csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly CommentsService _service;

        public CommentsController(CommentsService service)
        {
            _service = service;
        }

        // [HttpGet]
        // public ActionResult<Comment> GetAll()
        // {
        //     try
        //     {
        //         return Ok(_service.GetAll());
        //     }
        //     catch (System.Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        // }

    }
}