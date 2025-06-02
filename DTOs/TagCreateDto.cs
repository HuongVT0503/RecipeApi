using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    //DTO (Data Transfer Object): pattern dung de truyen du lieu giua cac layer
    //giup tach biet du lieu gui len tu client va entity trong database
    public class TagCreateDto
    {
        
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Tag name can only contain letters, numbers, spaces and hyphens")]
        public required string Name { get; set; }
    }
}
