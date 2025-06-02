namespace RecipeApi.DTOs
{
    //khac voi TagCreateDto, DTO nay bao gom ca Id va RecipeCount
    //RecipeCount duoc tinh tu so luong RecipeTags lien ket
    public class TagReadDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RecipeCount { get; set; }
    }
}

