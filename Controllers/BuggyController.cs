using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly DataContext _dataContext;

        public BuggyController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetSecret()
        {
            return "secret text";
        }
        
        [HttpGet("not-found")]
        public ActionResult<AppUser> GetNotFound()
        {
             return NotFound();

            
        }

        [HttpGet("server-error")]
        public ActionResult<string> GetServerError()
        {
            var thing = _dataContext.Users.Find(-1);
            var thingToReturn = this.ToString();
            throw new Exception("prueba");
            return BadRequest("Server say no mater facar");
        }
        [HttpGet("bad-request")]
        public ActionResult<string> GetBadRequest()
        {
          

            return BadRequest("given a bad request!");
        }

    }
}