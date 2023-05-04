using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BidditApi.Data;
using BidditApi.Models;
using Newtonsoft.Json.Linq;
using BidditApi.Utils;

namespace BidditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
          if (_context.Users == null)
          {
              return NotFound();
          }
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult> PostUser(User user)
        {
          if (_context.Users == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Users'  is null.");
          }
          var hashedPassword = PasswordHasher.Hash(user.Password);
            user.Password = hashedPassword;
            Console.WriteLine(user);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var jwtToken = JWTHasher.GenerateToken(user);
            Console.WriteLine("jwt" + jwtToken);
            var resObj = new
            {
                jwtToken,
                username = user.UserName,
                balance = user.WalletBalance
            };
            return CreatedAtAction("GetUser", new { id = user.UserId }, resObj);
        }

        // POST: api/Users/login
        [HttpPost("login")]
        public async Task<IActionResult> Login(User user)
        {

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName == user.UserName);
            if (existingUser == null)
            {
                return Unauthorized(); // Return 401 Unauthorized if login failed
            }
            var result = PasswordHasher.Verify(user.Password, existingUser.Password);
            if (!result)
            {
                return Unauthorized();
            }
            var jwtToken = JWTHasher.GenerateToken(existingUser);
            var resObj = new
            {
                jwtToken,
                username = user.UserName,
                balance = user.WalletBalance
            };
            return Ok(resObj); // Return the user object if login succeeded
        }

        //GET: api/Users/jwtValidate
        [HttpGet("jwtValidate")]
        public async Task<IActionResult> JwtValidate(string jwtToken)
        {
            var (isValid, value) = JWTHasher.ValidateToken(jwtToken);
            Console.WriteLine("isValid" + isValid);
            if (!isValid)
            {
                return Unauthorized();
            }

            Console.Write("value" + value);

            return Ok();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return (_context.Users?.Any(e => e.UserId == id)).GetValueOrDefault();
        }
    }
}
