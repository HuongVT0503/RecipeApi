using Humanizer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using RecipeApi.Data;
using RecipeApi.DTOs;
using RecipeApi.Models;
///////dont delegate, thats redundance n troublesome
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Threading.Tasks;


//CURRENTLY HAS ONLY CRUD: POST GET12 PUT DELETE

//MISSING TAG DTOs, TAGSCONTROLLER &RATINGS CONTROLLER+DTOs

//can add: serch term,, sort by date



namespace RecipeApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<RecipesController> _logger;
        
        //constructer  ->inject EFCore dbcontext
        public RecipesController(ApplicationDbContext context, ILogger<RecipesController> logger)
        {
            _context =context;
            _logger = logger;
        }






        // GET: api/Recipes?tag=Vegetarian //
        //GET /api/recipes                                  --> all recipes
        //GET /api/recipes?tag=Vegetarian                   -->all recipes with tag:Vegetarian. 
        //Dont support filtering w/ more than 2 tags
        // GET: api/Recipes?tag=Vegetarian&pageNumber=1&pageSize=10
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeReadDto>>> GetRecipes(
            [FromQuery] string? tag,
            [FromQuery] string? sortByRating = null)  // "asc" or "desc" 
        {
            // Base query: recipe->its tags & its ratings
            var query = _context.Recipes
                .Include(r => r.RecipeTags)
                       .ThenInclude(rt => rt.Tag)
                .Include(r => r.Ratings)
                .AsQueryable();

                //?filter for tag
            if (!string.IsNullOrWhiteSpace(tag))  //isnullorwhitespace filters for all isnullorempty filters for (null or ""), but also includes whitespces ("  ")
                    query = query.Where(r => r.RecipeTags.Any(rt => rt.Tag.Name == tag));

            //sort by rating: asc|| desc      //

            if (!string.IsNullOrWhiteSpace(sortByRating)){
                var asc = sortByRating.Equals("asc", StringComparison.OrdinalIgnoreCase);
                query=asc
                    ? query.OrderBy(r => r.Ratings.Any() ? r.Ratings.Average(x => x.Score) : 0)
                    : query.OrderByDescending(r => r.Ratings.Any() ? r.Ratings.Average(x => x.Score) : 0); //desc or des or anything is ok
            }


            var recipes = await query.ToListAsync();

                


                return Ok(new{
                    Data = recipes.Select(r => new RecipeReadDto
                    {
                        Id = r.Id,
                        Title = r.Title,
                        Instructions = r.Instructions,
                        Tags = r.RecipeTags.Select(rt => rt.Tag.Name).ToList(),
                        AverageRating = r.Ratings.Any() ? r.Ratings.Average(x => x.Score) : 0
                    })
                });
            
        }






        // GET: api/Recipes/{r.Id}
        //GET /api/recipes/5                                  ----> 1 recipe w/ id =5
        [HttpGet("{id:int}")]
        public async  Task<ActionResult<RecipeReadDto> > Get(int id)          //TASK: the framework run it asynchronously and resume when I/O completes
        {
            try
            {
                //load query: recipe->its tags &its ratings
                var recipe = await _context.Recipes
                       .Include(x => x.RecipeTags)
                              .ThenInclude(rt => rt.Tag)
                       .Include(x => x.Ratings)
                       .FirstOrDefaultAsync(x => x.Id == id);
                //await: await non-blocking  api calls, non-blocking io //await Task.Run() to do cpu-bound tasks on a backgr thread
                if (recipe == null) return NotFound();

                //map: 
                var dto = new RecipeReadDto 
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    Tags = recipe.RecipeTags.Select(rt => rt.Tag.Name).ToList(),
                    //AVERAGE ratings score 
                    AverageRating = recipe.Ratings.Any()        ? recipe.Ratings.Average(x => x.Score) : 0,
                };

                return Ok(dto);   //200 OK
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching recipe {RecipeId}", id);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }




        



        // PUT: api/Recipes/{id}
        //PUT /api/recipes/5                                       ---> UPDATE an EXISTING recipe by replacing its title,instructions,tags
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]                                                  
        public async Task<IActionResult> PutRecipe(int id, [FromBody] RecipeUpdateDto dto)         //<-- id comes from the url.
                                                                                                     //the dto obj(whochh holds in4 for the update) comes from the request body

        {
                //load 1 existing recipe, include its tags
               var exist= await _context.Recipes
                    .Include(r => r.RecipeTags)
                    .FirstOrDefaultAsync (r => r.Id == id); //firstordefault:async return first element of a sequence that satisfies the given condition, return default if else


                if (exist == null) return NotFound();      //404

                //UPDATE only if provided
                if (dto.Title != null)            exist.Title = dto.Title.Transform(To.TitleCase);
                if (dto.Instructions != null)       exist.Instructions = dto.Instructions;
         
                if (dto.TagIds != null)  //dto contains the new tags
                {
                    exist.RecipeTags = dto.TagIds
                        .Select(tagId => new RecipeTag { RecipeId = id, TagId = tagId })
                        //each new RecipeTag connects the current recipe to one of the new tags in dto.TagIds

                        .ToList();
                }

                await _context.SaveChangesAsync();

                return NoContent();
            
        }









        // POST: api/Recipes
        //POST /api/recipes                                           --->create 1 new recipe from a RecipeCreateDto
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<RecipeReadDto>> PostRecipe([FromBody] RecipeCreateDto dto)
        {
            try
            {
                //tag entity for given id
                var tags =  await _context.Tags
                      .Where(t=> dto.TagIds.Contains(t.Id))       // WHERE Tag.Id IN (...) =>find existing tags to give the new recipe
                      .ToListAsync();
                


                //new Recipe entity from incoming dto
                var recipe = new Recipe{
                    Title = dto.Title.Transform(To.TitleCase),      //make all input tiles be title case
                    Instructions = dto.Instructions,

                    RecipeTags=tags.Select(t=> new RecipeTag { Tag=t}).ToList(),   //RecipeTags

                   
                };



                //add that new entity to context +save to db
                _context.Recipes.Add(recipe);
                await _context.SaveChangesAsync();


                //then load related tags to populate the read DTO
                await _context.Entry(recipe)       //(EF Change Tracker) find the just‐inserted recipe instance

                    .Collection(r => r.RecipeTags)        //access its RecipeTags
                    .Query()                                
                    .Include(rt => rt.Tag)            //modify the query to include rt.Tag
                    .LoadAsync();

                // Build read DTO to return
                var readDto = new RecipeReadDto
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    Tags = recipe.RecipeTags.Select(rt => rt.Tag.Name).ToList(),         //rt.Tag.Name
                    AverageRating = 0,          //new->no ratings
                };


                return CreatedAtAction(nameof(PostRecipe),
                    new { id = recipe.Id,},
                    readDto);
                //return 201 Created     Location header points to PostRecipe
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating recipe");
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }













        // DELETE: api/Recipes/{r.id}
        //DELETE /api/recipes/5   -->delete recipe w/ id=5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            try
            {
                //find recipe by id
                var recipe = await _context.Recipes
                    .Include(r => r.RecipeTags)
                    .Include(r => r.Ratings)
                    .FirstOrDefaultAsync(r => r.Id == id);
                if (recipe == null)
                {
                    return NotFound();
                }

                // Xoa cac quan he truoc
                recipe.RecipeTags.Clear();
                recipe.Ratings.Clear();

                _context.Recipes.Remove(recipe);           //delete
                await _context.SaveChangesAsync();         //save

                return NoContent();  //204 No Content
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting recipe {RecipeId}", id);
                return StatusCode(500, new { error = "An error occurred while processing your request", details = ex.Message });
            }
        }

        private async Task<bool> RecipeExists(int id)
        {
            return await _context.Recipes.AnyAsync(e => e.Id == id);
        }
    }
}
