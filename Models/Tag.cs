using System.Collections.Generic;

namespace RecipeApi.Models
{
    public class Tag
    {
        public int Id { get; set; }   //pk

        public string Name { get; set; }

        public  ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
