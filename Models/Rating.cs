using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models
{
    public class Rating
    {
        public int Id { get; set; }   //pk
        public int RecipeId { get; set; }  //fk
        public Recipe Recipe { get; set; }
        public int Score { get; set; } //1-5
    }
}
