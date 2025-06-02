using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeApi.Data;
using RecipeApi.DTOs;
using RecipeApi.Models;
using Humanizer;

namespace RecipeApi.Controllers
{
    // Route mac dinh cho controller la /api/Tags
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        //dbContext de truy cap database
        private readonly ApplicationDbContext _context;
        private readonly ILogger<TagsController> _logger;

        public TagsController(ApplicationDbContext context, ILogger<TagsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Tags
        //lay danh sach tat ca tag, co phan trang
        [HttpGet]

        [ResponseCache(Duration = 300)] // Cache for 5 minutes since tags don't change often
        public async Task<ActionResult<IEnumerable<TagReadDto>>> GetTags()
        {
            var tags = await _context.Tags
                    .Select(t => new TagReadDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        RecipeCount = t.RecipeTags.Count
                    })
                    .ToListAsync();
             return Ok(new{ Data = tags});
            
        }

        // GET: api/Tags/5
        // Lay thong tin chi tiet cua mot tag theo id
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<TagReadDto>> GetTag(int id)
        {
            if (id <= 0)           return BadRequest("Invalid tag ID");
            var tag = await _context.Tags
                    .Include(t => t.RecipeTags)
                    .FirstOrDefaultAsync(t => t.Id == id);

            if (tag == null)  return NotFound($"Tag with ID {id} not found");
            
            var tagDto = new TagReadDto{
                    Id = tag.Id,
                    Name = tag.Name,
                    RecipeCount = tag.RecipeTags.Count
            };

            return tagDto;
            
        }

        // PUT: api/Tags/5
        // Cap nhat thong tin cua mot tag
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagUpdateDto tagDto)
        {
            if (id <= 0)         return BadRequest("Invalid tag ID");
            
            if (tagDto == null)  return BadRequest("Tag data is required");

            if (string.IsNullOrWhiteSpace(tagDto.Name))           return BadRequest("Tag name cannot be empty");
                
            var tag = await _context.Tags.FindAsync(id);
                if (tag == null) return NotFound($"Tag with ID {id} not found");
               
                tag.Name = tagDto.Name.Trim().Titleize();

             
            await _context.SaveChangesAsync();
                
            if (!TagExists(id)) return NotFound();

            return NoContent();
            
        }

        // POST: api/Tags
        // Tao tag moi
        [HttpPost]
        public async Task<ActionResult<TagReadDto>> PostTag(TagCreateDto tagDto)
        {
            
                if (tagDto == null) return BadRequest("Tag data is required");
          
                if (string.IsNullOrWhiteSpace(tagDto.Name))           return BadRequest("Tag name cannot be empty");
                var titleizedName = tagDto.Name.Trim().Titleize();

                if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == titleizedName.ToLower()))  
                               return Conflict($"A tag with name '{titleizedName}' already exists");
               
                var tag =new Tag
                {
                    Name = titleizedName,
                    RecipeTags = new List<RecipeTag>()
                };

                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                var createdTag = new TagReadDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    RecipeCount = 0
                };

                //201 created
                return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, createdTag);
            
        }






        // DELETE: api/Tags/5
        // Xoa mot tag
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            if (id <= 0)   return BadRequest("Invalid tag ID");

            var tag = await _context.Tags
                .Include(t => t.RecipeTags)
                .FirstOrDefaultAsync(t => t.Id == id);
            
            if (tag == null)   return NotFound($"Tag with ID {id} not found");
            if (tag.RecipeTags.Any())
                    return BadRequest(new { error = "Cannot delete tag", details = "This tag is associated with one or more recipes" });

            //xoa tag
            _context.Tags.Remove(tag);
            await _context.SaveChangesAsync();

            return NoContent();
           
        }

        //helper
        private bool TagExists(int id)
        { return _context.Tags.Any(e => e.Id == id);
        }
    }
}
