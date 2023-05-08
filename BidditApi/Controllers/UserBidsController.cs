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

namespace BidditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserBidsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserBidsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/UserBids
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserBids>>> GetUserBids()
        {
          if (_context.UserBids == null)
          {
              return NotFound();
          }
            return await _context.UserBids.ToListAsync();
        }

        // GET: api/UserBids/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserBids>> GetUserBids(int id)
        {
          if (_context.UserBids == null)
          {
              return NotFound();
          }
            var userBids = await _context.UserBids.FindAsync(id);

            if (userBids == null)
            {
                return NotFound();
            }

            return userBids;
        }

        // PUT: api/UserBids/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUserBids(int id, UserBids userBids)
        {
            if (id != userBids.Id)
            {
                return BadRequest();
            }

            _context.Entry(userBids).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserBidsExists(id))
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

        // POST: api/UserBids
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<UserBids>> PostUserBids(UserBidsCustom bid)
        {
          if (_context.UserBids == null)
          {
              return Problem("Entity set 'ApplicationDbContext.UserBids'  is null.");
          }


            var bidExists = _context.Bids.Find(bid.BidId);

            if(bidExists == null)
            {
                return BadRequest("Bid Doesnt Exist");
            }

            var UserId = HttpContext.Items["UserId"] as string;
            if(UserId == null)
            {
                return Unauthorized();
            }
            var userBids = new UserBids
            {
                UserId = Int32.Parse(UserId),
                BidId = bid.BidId,
                BidAmount = bid.BidAmount
            };


            _context.UserBids.Add(userBids);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetUserBids", new { id = userBids.Id }, userBids);
        }

        // DELETE: api/UserBids/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserBids(int id)
        {
            if (_context.UserBids == null)
            {
                return NotFound();
            }
            var userBids = await _context.UserBids.FindAsync(id);
            if (userBids == null)
            {
                return NotFound();
            }

            _context.UserBids.Remove(userBids);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserBidsExists(int id)
        {
            return (_context.UserBids?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public class UserBidsCustom
        {
            public int BidId { get; set; }
            public int BidAmount { get; set; }
        } 
    }
}
