using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs
{
    /// <summary>
    /// DTO for creating a new tag
    /// </summary>
    // DTO nay dung de tao tag moi
    // DTO (Data Transfer Object) la mot pattern dung de truyen du lieu giua cac layer
    // Trong truong hop nay, no giup tach biet du lieu gui len tu client va entity trong database
    public class TagCreateDto
    {
        // Ten cua tag
        // Required: Bat buoc phai co gia tri
        // StringLength: Do dai chuoi phai tu 2 den 50 ky tu
        // RegularExpression: Chi cho phep chu cai, so, khoang trang va dau gach ngang
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Tag name can only contain letters, numbers, spaces and hyphens")]
        public required string Name { get; set; }
    }
}
