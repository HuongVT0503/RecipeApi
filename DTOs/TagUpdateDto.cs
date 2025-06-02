using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    //tuong tu TagCreateDto nhung duoc su dung cho PUT request
    //khong can Id vi Id duoc truyen qua route parameter
    public class TagUpdateDto
    {
        [Required]
        public string Name { get; set; }
    }
} 