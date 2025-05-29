using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecipeApi.Models
{
    // Model Tag dai dien cho the loai cong thuc nau an
    // Vi du: Mon chay, Mon man, Mon cay, etc.
    public class Tag
    {
        // Id la primary key cua bang Tags
        public int Id { get; set; }   //pk

        // Ten cua tag, bat buoc phai co
        [Required(ErrorMessage = "Tag name is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Tag name must be between 2 and 50 characters")]
        [RegularExpression(@"^[a-zA-Z0-9\s\-]+$", ErrorMessage = "Tag name can only contain letters, numbers, spaces and hyphens")]
        //[Column(TypeName = "nvarchar(50)")]
        public required string Name { get; set; }

        // Danh sach cac RecipeTag lien ket
        // Day la navigation property de quan ly quan he many-to-many voi Recipe
        public required ICollection<RecipeTag> RecipeTags { get; set; }
    }
}
