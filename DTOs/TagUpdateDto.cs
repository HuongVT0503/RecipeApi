using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    /// <summary>
    /// DTO for updating an existing tag
    /// </summary>
    // DTO nay dung de cap nhat tag da ton tai
    // Tuong tu nhu TagCreateDto nhung duoc su dung cho PUT request
    // Khong can Id vi Id duoc truyen qua route parameter
    public class TagUpdateDto
    {
        // Ten moi cua tag
        // Co cung validation nhu TagCreateDto
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Tag name can only contain letters, numbers, spaces and hyphens")]
        public required string Name { get; set; }
    }
} 