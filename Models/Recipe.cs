using System.Collections.Generic;

namespace RecipeApi.Models
{
    // Model Recipe dai dien cho cong thuc nau an
    public class Recipe
    {
        // Id la primary key cua bang Recipes
        public int Id { get; set; }     //pk

        // Tieu de cua cong thuc
        public string Title { get; set; }

        // Huong dan nau an
        public string Instructions { get; set; }

        // Danh sach cac RecipeTag lien ket
        // Navigation property de quan ly quan he many-to-many voi Tag
        public ICollection<RecipeTag> RecipeTags { get; set; }   //navigate

        // Danh sach cac Rating cua cong thuc
        // Navigation property de quan ly quan he one-to-many voi Rating
        public ICollection<Rating> Ratings { get; set; }
    }
}
