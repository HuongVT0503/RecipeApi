using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    public class RecipeUpdateDto
    {
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Title must be between 3 and 200 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,!?()]+$", ErrorMessage = "Title can only contain letters, numbers, spaces, and basic punctuation (-_.,!?())")]
        public string? Title { get; set; }

        [MinLength(10, ErrorMessage = "Instructions must be at least 10 characters long")]
        public string? Instructions { get; set; }

        public List<int>? TagIds { get; set; }
    }
} 