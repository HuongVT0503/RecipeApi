namespace RecipeApi.Models
{
    // Model RecipeTag la bang trung gian de quan ly quan he many-to-many
    // giua Recipe va Tag
    public class RecipeTag
    {
        // Id cua cong thuc
        // Foreign key lien ket voi bang Recipes
        public int RecipeId { get; set; }   //fk

        // Navigation property de quan ly quan he voi Recipe
        public Recipe Recipe { get; set; }

        // Id cua tag
        // Foreign key lien ket voi bang Tags
        public int TagId { get; set; }    //fk

        // Navigation property de quan ly quan he voi Tag
        public Tag Tag { get; set; }   //navigate
    }
}
