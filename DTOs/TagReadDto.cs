namespace RecipeApi.DTOs
{
    /// <summary>
    /// DTO for reading tag information
    /// </summary>
    // DTO nay dung de doc thong tin cua tag
    // Khac voi TagCreateDto, DTO nay bao gom ca Id va RecipeCount
    // RecipeCount duoc tinh toan tu so luong RecipeTags lien ket
    public class TagReadDto
    {
        // Id cua tag trong database
        public int Id { get; set; }

        // Ten cua tag
        public required string Name { get; set; }

        // So luong cong thuc nau an duoc gan voi tag nay
        // Duoc tinh toan bang cach dem so luong RecipeTags
        public int RecipeCount { get; set; }
    }
}

