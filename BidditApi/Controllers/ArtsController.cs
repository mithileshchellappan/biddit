using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BidditApi.Data;
using BidditApi.Models;
using Microsoft.AspNetCore.StaticFiles;

namespace BidditApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ArtsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Arts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<dynamic>>> GetArts([FromQuery] bool promotedOnly = false,bool userImages = false)
        {
            var query = from art in _context.Arts
                                    join user in _context.Users on art.UserId equals user.UserId into users
                                    from user in users.DefaultIfEmpty()
                                    select new
                                    {
                                        art.ArtId,
                                        art.Title,
                                        art.UserId,
                                        art.Description,
                                        art.ArtURL,
                                        art.IsPromoted,
                                        UserName = user != null ? user.UserName : null
                                    };


            if (promotedOnly)
            {
                
                query = query.Where(a => a.IsPromoted);
            }
            if (userImages)
            {
            var UserId = HttpContext.Items["UserId"] as string;
                query = query.Where(a => a.UserId == Int32.Parse(UserId));
            }
            var arts = await query.ToListAsync();
            Console.WriteLine(arts);
            if(arts == null)
            {
                return NotFound();
            }
            return arts;
        }

        // GET: api/Arts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Art>> GetArt(int id)
        {
          if (_context.Arts == null)
          {
              return NotFound();
          }
            var art = await _context.Arts.FindAsync(id);

            if (art == null)
            {
                return NotFound();
            }

            return art;
        }

        // PUT: api/Arts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArt(int id, Art art)
        {
            if (id != art.ArtId)
            {
                return BadRequest();
            }

            _context.Entry(art).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArtExists(id))
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

        // POST: api/Arts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostArt(ArtInputModel input)
        {
   
            Console.WriteLine("Inside");
          if (_context.Arts == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Arts'  is null.");
          }
            var UserId = HttpContext.Items["UserId"] as string;
            Console.WriteLine("Here"+UserId);
            Art art = new Art
            {
                ArtURL = input.ArtURL,
                Title = input.Title,
                Description = input.Description,
                CreatedDate = DateTime.Now,
                UserId = Int32.Parse(UserId)
            };
            
            Console.WriteLine(art.UserId);
          _context.Arts.Add(art);
          await _context.SaveChangesAsync();

            return CreatedAtAction("GetArt", new { id = art.ArtId }, art);
        }
        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        [HttpPost("uploadFile")]
        public async Task<ActionResult<object>> UploadImage(IFormFile formFile)
        {
            if (formFile == null || formFile.Length == 0)
            {
                return Problem("Entity set 'ApplicationDbContext.Arts'  is null.");
            }

            var fileId = Guid.NewGuid();
            var fileName = RandomString(10);
            var ext = System.IO.Path.GetExtension(formFile.FileName);
            fileName = fileName + ext;
            var path = Path.Combine(
                Directory.GetCurrentDirectory(), "uploads", fileName);
            using (var stream = new FileStream(path, FileMode.Create))
                await formFile.CopyToAsync(stream);

            Console.WriteLine(fileName);
            var resObj = new
            {
                fileName = fileName
            };
            return resObj; 
        }

        [HttpGet("getFile")]
        public async Task<IActionResult> GetImage(string fileName)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "uploads", fileName);

            if (!System.IO.File.Exists(path))
            {
                return NotFound();
            }

            var fileContent = await System.IO.File.ReadAllBytesAsync(path);
            var contentType = GetMimeType(path);

            return File(fileContent, contentType);
        }

        private string GetMimeType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(path, out contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }




        // DELETE: api/Arts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArt(int id)
        {
            if (_context.Arts == null)
            {
                return NotFound();
            }
            var art = await _context.Arts.FindAsync(id);
            if (art == null)
            {
                return NotFound();
            }

            _context.Arts.Remove(art);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArtExists(int id)
        {
            return (_context.Arts?.Any(e => e.ArtId == id)).GetValueOrDefault();
        }

        public class ArtInputModel
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string ArtURL { get; set; }
        }

        //public class ArtUserModel
        //{
        //   public int ArtId { get; set; }
        //   public string Title { get; set; }
        //   public int UserId { get; set; }
        //   public String Description { get; set; }
        //   public String    ArtURL { get; set; }
        //   public bool IsPromoted { get; set; }
        //   public string UserName { get; set; }
        //}
    }
}
