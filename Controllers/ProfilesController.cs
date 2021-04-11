using System.Collections.Generic;
using bloggr_csharp.Models;
using bloggr_csharp.Services;
using Microsoft.AspNetCore.Mvc;

namespace bloggr_csharp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfilesController : ControllerBase
    {
        private readonly ProfilesService _pservice;
        public ProfilesController(ProfilesService pservice)
        {
            _pservice = pservice;
        }

        [HttpGet("{id}")]
        public ActionResult<Profile> Get(string id)
        {
            try
            {
                return Ok(_pservice.GetProfileById(id));
            }
            catch (System.Exception err)
            {
                return BadRequest(err.Message);
            }
        }

    }
}