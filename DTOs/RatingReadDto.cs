namespace RecipeApi.DTOs
{
    public class RatingReadDto
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int Score { get; set; }
    }
} 