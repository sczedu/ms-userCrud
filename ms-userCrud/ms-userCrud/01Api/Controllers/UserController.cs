using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms_userCrud._01Api.Model;
using ms_userCrud._02Service;
using System;

namespace ms_userCrud.Controllers
{
    [Route("api/v1/user")]
    [Produces("application/json")]
    [ApiController]
    [Authorize("Bearer")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("Authentication")]
        public IActionResult Authentication([FromBody]User user)
        {
            try
            {
                var token = _userService.Authentication(user);
                if (token == null)
                    new NotFoundResult();

                return new OkObjectResult(token);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [AllowAnonymous]
        [HttpPost("insert")]
        public IActionResult InsertUser([FromBody] User user)
        {
            try
            {
                var ok = _userService.InsertUser(user);
                if(ok==1)
                    return new CreatedResult($"api/v1/user/{user.Id}", user.Id);
                return BadRequest();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }


        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, User user)
        {
            try
            {
                var userSaved = _userService.UpdateUser(id, user);

                if (userSaved == null)
                    return new NotFoundResult();

                return new OkObjectResult(userSaved);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = _userService.GetById(id);

                if (user == null)
                    return NotFound();

                _userService.DeleteUser(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return new NoContentResult();
        }

        [HttpGet]
        public IActionResult List()
        {
            try
            {
                var result = _userService.List();
                return new OkObjectResult(result);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                var result = _userService.GetById(id);
                if (result == null)
                    return new NotFoundResult();

                return new OkObjectResult(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }


    }
}
