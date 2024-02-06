using ibay.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using ibay.Filters;
using ibay.Services;


namespace ibay.Controllers
{
    [Route("/api/user/")]
    [ApiController]
    [ExecutedReqFilter]

    public class UserController: ControllerBase
    {
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(400)]
        [Route("getall")]
        [EnableCors("MyCustomPolicy")]
        public IActionResult Get(IIbay ibay, [FromBody] int limit)
        {
            return Ok(ibay.GetUsers(limit));
        }

        [HttpGet]
        [Route("getby")]
        public IActionResult Get(IIbay ibay, [FromBody] ItemId itemId)
        {
            var id = itemId.Id;
            var User = ibay.GetUserById(id);
            if (User == null) { return NotFound($"L'étudiant d'id {id} n'a pas été trouvé"); }

            return Ok(User);
        }

        [HttpPost]
        [Route("new")]
        public IActionResult Post(IIbay ibay, [FromBody] User User)
        {
            try
            {
                var id = ibay.CreateUser(User);

                return Created($"/api/stud/{id}", id);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        [Route("update")]
        public IActionResult Put(IIbay ibay, [FromBody] User User)
        {
            ibay.UpdateUser(User.Id, User);
            return Ok();
        }

        [HttpDelete]
        [Route("delete")]
        public IActionResult Delete(IIbay ibay, [FromBody] ItemId itemId)
        {
            var id = itemId.Id;
            ibay.DeleteUser(id);
            return Ok();
        }


        
    }
}
    