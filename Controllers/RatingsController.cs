using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RecipeApi.Data;
using RecipeApi.DTOs;
using RecipeApi.Models;

namespace RecipeApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(ApplicationDbContext context, ILogger<RatingsController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST: api/Ratings
        [HttpPost]
        public async Task<ActionResult<RatingReadDto>> CreateRating(RatingCreateDto ratingDto)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(ratingDto.RecipeId);
                if (recipe == null)
                {
                    return NotFound(new { error = "Recipe not found", details = $"No recipe exists with ID {ratingDto.RecipeId}" });
                }

                var rating = new Rating
                {
                    RecipeId = ratingDto.RecipeId,
                    Score = ratingDto.Score,
                    Recipe = recipe
                };

                _context.Ratings.Add(rating);
                await _context.SaveChangesAsync();

                var ratingReadDto = new RatingReadDto
                {
                    Id = rating.Id,
                    RecipeId = rating.RecipeId,
                    Score = rating.Score
                };

                return Ok(ratingReadDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating rating for recipe {RecipeId}", ratingDto.RecipeId);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        // GET: api/Ratings/recipe/5
        [HttpGet("recipe/{recipeId}")]
        public async Task<ActionResult<IEnumerable<RatingReadDto>>> GetRecipeRatings(int recipeId)
        {
            try
            {
                var recipe = await _context.Recipes.FindAsync(recipeId);
                if (recipe == null)
                {
                    return NotFound(new { error = "Recipe not found", details = $"No recipe exists with ID {recipeId}" });
                }

                var ratings = await _context.Ratings
                    .Where(r => r.RecipeId == recipeId)
                    .Select(r => new RatingReadDto
                    {
                        Id = r.Id,
                        RecipeId = r.RecipeId,
                        Score = r.Score
                    })
                    .ToListAsync();

                return ratings;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching ratings for recipe {RecipeId}", recipeId);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }
    }
} 