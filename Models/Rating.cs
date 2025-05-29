namespace RecipeApi.Models
{
    // Model Rating dai dien cho danh gia cua cong thuc nau an
    public class Rating
    {
        // Id la primary key cua bang Ratings
        public int Id { get; set; }   //pk

        // Id cua cong thuc duoc danh gia
        // Foreign key lien ket voi bang Recipes
        public int RecipeId { get; set; }  //fk

        // Navigation property de quan ly quan he voi Recipe
        public required Recipe Recipe { get; set; }

        // Diem danh gia tu 1-5
        public int Score { get; set; } //1-5
    }
}
