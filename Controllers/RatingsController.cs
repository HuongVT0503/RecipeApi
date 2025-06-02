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
    public class RatingsController: ControllerBase
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
        public async Task<ActionResult<RatingReadDto>> CreateRating(RatingCreateDto rating)
        {
            var recipe = await _context.Recipes.FindAsync(rating.RecipeId);
            if (recipe == null)    return NotFound();   ///
            var newRating = new Rating{
                RecipeId = rating.RecipeId,
                Score = rating.Score,
                Recipe = recipe
            };

            _context.Ratings.Add(newRating);
            await _context.SaveChangesAsync();

            return Ok(new RatingReadDto{
                Id = newRating.Id,
                RecipeId = newRating.RecipeId,
                Score = newRating.Score
            });
            
            
        }







        // GET: api/Ratings/recipe/5
        [HttpGet("recipe/{recipeId}")]
        public async Task<ActionResult<IEnumerable<RatingReadDto>>> GetRecipeRatings(int recipeId)
        {
            var recipe = await _context.Recipes.FindAsync(recipeId);
            if (recipe == null)          return NotFound();

            
            var ratings = await _context.Ratings
                    .Where(r => r.RecipeId == recipeId)
                    .Select(r => new RatingReadDto
                    {
                        Id = r.Id,
                        RecipeId = r.RecipeId,
                        Score = r.Score
                    })
                    .ToListAsync();
            return Ok(ratings);
            
            
        }
    }
} 