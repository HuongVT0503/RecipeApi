using System.ComponentModel.DataAnnotations;

namespace RecipeApi.Models
{
    public class RecipeTag
    {
        public int RecipeId { get; set; }   //fk
        public Recipe Recipe { get; set; }

        public int TagId { get; set; }    //fk

        //navigation property
        public Tag Tag { get; set; } 
    }
}
