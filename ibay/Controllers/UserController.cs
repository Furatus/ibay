using ibay.Model;
using ibay.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
// using ibay.Filters;


namespace ibay.Controllers;

public class UserController
{
    [Route("api/user/")]
    // [ApiController]
    
    [HttpGet]
    [Produces("application/json")]
    [ProducesResponseType(400)]
    [Route("getall")]
    [EnableCors("MyCustomPolicy")]
    public IActionResult Get(IIbay ibay)
    {
        return Ok(ibay.GetUsers());
    }

    [HttpGet]
    [Route("getby")]
    public IActionResult Get(IIbay ibay, int id)
    {
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
    public IActionResult Put(IIbay ibay, int id, [FromBody] User User)
    {
        ibay.UpdateUser(id, User);
        return Ok();
    }

    [HttpDelete]
    [Route("delete")]
    public IActionResult Delete(IIbay ibay, int id)
    {
        ibay.RemoveUser(id);
        return Ok();
    }


    
}