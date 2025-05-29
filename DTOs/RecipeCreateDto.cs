using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs      //DATA TRANSFER OBJECT
{
    public class RecipeCreateDto{
        [Required(ErrorMessage= "Title is required")]
        [StringLength(200, MinimumLength = 3, ErrorMessage= "Title must be between 3 and 200 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-_.,!?()]+$", ErrorMessage= "Title can only contain letters, numbers, spaces, and basic punctuation (-_.,!?())")]
        public required string Title { get; set; }

        [Required(ErrorMessage= "Instructions are required")]
        [MinLength(10, ErrorMessage= "Instructions must be at least 10 characters long")]
        public required string Instructions { get; set; }

        [Required(ErrorMessage = "At least one tag is required")]
        [MinLength(1, ErrorMessage = "At least one tag is required")]
        public required List<int> TagIds { get; set; }
    }



}
