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
        // DbContext de truy cap database
        private readonly ApplicationDbContext _context;
        // Logger de ghi log
        private readonly ILogger<TagsController> _logger;

        // Constructor injection: Nhan DbContext va Logger tu DI container
        public TagsController(ApplicationDbContext context, ILogger<TagsController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        // GET: api/Tags
        //lay danh sach tat ca tag, co phan trang
        [HttpGet]

        //cache for ONLY 5 SECS only for testing purposes
        [ResponseCache(Duration = 300)] // Cache for 5 minutes since tags don't change often
        public async Task<ActionResult<IEnumerable<TagReadDto>>> GetTags(/*[FromQuery] int page = 1, [FromQuery] int pageSize = 10*/)
        {
            try
            {
                /*if (page < 1)
                {
                    return BadRequest("Page number must be greater than 0");
                }

                if (pageSize < 1 || pageSize > 100)
                {
                    return BadRequest("Page size must be between 1 and 100");
                }*/
                // Query lay tag voi phan trang
                // Include: Load them RecipeTags de tinh RecipeCount
                // Skip: Bo qua cac record o trang truoc
                // Take: Lay so record theo pageSize
                // Select: Chuyen doi sang TagReadDto
                // Get total count for pagination metadata
                /*var totalCount = await _context.Tags.CountAsync();
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);*/

                // Optimize query by using a more efficient count
                var tags = await _context.Tags
                    .Select(t => new TagReadDto
                    {
                        Id = t.Id,
                        Name = t.Name,
                        RecipeCount = t.RecipeTags.Count
                    })
                    /*.Skip((page - 1) * pageSize)
                    .Take(pageSize)*/
                    .ToListAsync();

                /*if (!tags.Any() && page > 1)
                {
                    return NotFound($"No tags found on page {page}");
                }*/

                // Add pagination metadata to response
                /*var paginationMetadata = new
                {
                    TotalCount = totalCount,
                    PageSize = pageSize,
                    CurrentPage = page,
                    TotalPages = totalPages,
                    HasNext = page < totalPages,
                    HasPrevious = page > 1
                };*/

                return Ok(new
                {
                    Data = tags
                    /*Pagination = paginationMetadata*/
                });
            }
            catch (Exception ex)
            {
                // Ghi log loi va tra ve 500 Internal Server Error
                _logger.LogError(ex, "Error occurred while fetching tags");
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // GET: api/Tags/5
        // Lay thong tin chi tiet cua mot tag theo id
        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)]
        public async Task<ActionResult<TagReadDto>> GetTag(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid tag ID");
                }

                // Tim tag theo id, load them RecipeTags
                var tag = await _context.Tags
                    .Include(t => t.RecipeTags)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tag == null)
                {
                    return NotFound($"Tag with ID {id} not found");
                }

                // Chuyen doi sang TagReadDto
                var tagDto = new TagReadDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    RecipeCount = tag.RecipeTags.Count
                };

                return tagDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching tag {TagId}", id);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // PUT: api/Tags/5
        // Cap nhat thong tin cua mot tag
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTag(int id, TagUpdateDto tagDto)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid tag ID");
                }

                if (tagDto == null)
                {
                    return BadRequest("Tag data is required");
                }

                if (string.IsNullOrWhiteSpace(tagDto.Name))
                {
                    return BadRequest("Tag name cannot be empty");
                }

                // Tim tag can cap nhat
                var tag = await _context.Tags.FindAsync(id);
                if (tag == null)
                {
                    return NotFound($"Tag with ID {id} not found");
                }

                // Cap nhat ten tag with Title Case
                tag.Name = tagDto.Name.Trim().Titleize();

                try
                {
                    // Luu thay doi vao database
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    // Kiem tra xem tag co con ton tai khong
                    if (!TagExists(id))
                    {
                        return NotFound($"Tag with ID {id} no longer exists");
                    }
                    _logger.LogError(ex, "Concurrency error while updating tag {TagId}", id);
                    return StatusCode(409, "The tag was modified by another user. Please refresh and try again.");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating tag {TagId}", id);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // POST: api/Tags
        // Tao tag moi
        [HttpPost]
        public async Task<ActionResult<TagReadDto>> PostTag(TagCreateDto tagDto)
        {
            try
            {
                if (tagDto == null)
                {
                    return BadRequest("Tag data is required");
                }

                if (string.IsNullOrWhiteSpace(tagDto.Name))
                {
                    return BadRequest("Tag name cannot be empty");
                }

                // Titleize the tag name
                var titleizedName = tagDto.Name.Trim().Titleize();

                // Check if tag with same name already exists
                if (await _context.Tags.AnyAsync(t => t.Name.ToLower() == titleizedName.ToLower()))
                {
                    return Conflict($"A tag with name '{titleizedName}' already exists");
                }

                // Tao entity Tag moi tu DTO
                var tag = new Tag
                {
                    Name = titleizedName,
                    RecipeTags = new List<RecipeTag>()
                };

                // Them vao database
                _context.Tags.Add(tag);
                await _context.SaveChangesAsync();

                // Tao TagReadDto de tra ve
                var createdTag = new TagReadDto
                {
                    Id = tag.Id,
                    Name = tag.Name,
                    RecipeCount = 0
                };

                // Tra ve 201 Created voi location header
                return CreatedAtAction(nameof(GetTag), new { id = tag.Id }, createdTag);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating tag");
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // DELETE: api/Tags/5
        // Xoa mot tag
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTag(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid tag ID");
                }

                // Tim tag can xoa, load them RecipeTags
                var tag = await _context.Tags
                    .Include(t => t.RecipeTags)
                    .FirstOrDefaultAsync(t => t.Id == id);

                if (tag == null)
                {
                    return NotFound($"Tag with ID {id} not found");
                }

                // Kiem tra xem tag co duoc su dung trong recipe nao khong
                if (tag.RecipeTags.Any())
                {
                    return BadRequest(new { error = "Cannot delete tag", details = "This tag is associated with one or more recipes" });
                }

                // Xoa tag
                _context.Tags.Remove(tag);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting tag {TagId}", id);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // Helper method: Kiem tra xem tag co ton tai khong
        private bool TagExists(int id)
        {
            return _context.Tags.Any(e => e.Id == id);
        }
    }
}
