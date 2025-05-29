namespace RecipeApi.DTOs
{
    public class RecipeReadDto
    {
        public required int Id { get; set; }
        public required string Title { get; set; }
        public required string Instructions { get; set; }
        public required List<string> Tags { get; set; }
        public double AverageRating { get; set; }   //
    }

}
