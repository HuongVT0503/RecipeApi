using System.Collections.Generic;

namespace RecipeApi.DTOs
{
    public class RecipeReadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
        public List<string> Tags { get; set; }
        public double AverageRating { get; set; }
    }
}
