using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ms_userCrud.Business;
using ms_userCrud.Data;
using ms_userCrud.Models;
using ms_userCrud.Models.Vallidator;
using System;
using System.Linq;

namespace ms_userCrud.Controllers
{
    [Route("api/v1/user")]
    [Produces("application/json")]
    [ApiController]
    [Authorize("Bearer")]
    public class UserController : ControllerBase
    {
        private readonly MysqlDBContext _context;

        public UserController(MysqlDBContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        [HttpPost("Authenticate")]
        public IActionResult Authentication([FromBody]User user, [FromServices]PasswordService passwordBusiness)
        {
            try
            {
                var hashPassword = passwordBusiness.GetHash(user.Password);
                var userDb = _context.User.FirstOrDefault(f => f.Username == user.Username && f.Password == hashPassword);
                if (userDb == null)
                    return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return new OkObjectResult(passwordBusiness.GenerateToken(user));
        }

        [AllowAnonymous]
        [HttpPost("insert")]
        public IActionResult InsertUser([FromBody] User user, [FromServices]PasswordService passwordBusiness)
        {
            int idInsert;
            try
            {
                new UserValidator().Validate(user);
                user.Password = passwordBusiness.GetHash(user.Password);
                _context.User.Add(user);
                idInsert = _context.SaveChanges();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return new OkObjectResult(idInsert);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] User user, [FromServices]PasswordService passwordBusiness)
        {
            try
            {
                var oldUser = _context.User.FirstOrDefault(f => f.Id == id);
                if (oldUser == null)
                    return NotFound();
                if (oldUser.Password != user.Password)
                    oldUser.Password = passwordBusiness.GetHash(user.Password);
                oldUser.Document = user.Document;
                oldUser.Email = user.Email;
                oldUser.Name = user.Name;
                _context.User.Update(oldUser);
                _context.SaveChanges();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex);
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }

            return new ObjectResult(user);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var userDb = _context.User.FirstOrDefault(f => f.Id == id);
                if (userDb == null)
                    return NotFound();

                _context.User.Remove(userDb);
                _context.SaveChanges();
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
                return new ObjectResult(_context.User.ToList());
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
                var result = _context.User.FirstOrDefault(f => f.Id == id);
                if (result == null)
                    return NotFound();

                return new ObjectResult(result);
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
