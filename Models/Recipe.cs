using System.Collections.Generic;

namespace RecipeApi.Models
{
    public class Recipe
    {
        public int Id { get; set; }     //pk
        public string Title { get; set; }
        public string Instructions { get; set; }
        public ICollection<RecipeTag> RecipeTags { get; set; }   //navigate
        public ICollection<Rating> Ratings { get; set; }
    }
}
