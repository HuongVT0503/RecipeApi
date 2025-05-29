using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    public class RatingCreateDto
    {
        [Required]
        public int RecipeId { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Score { get; set; }
    }
} 