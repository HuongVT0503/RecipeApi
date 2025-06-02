using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RecipeApi.DTOs      //DATA TRANSFER OBJECT
{
    public class RecipeCreateDto
    {
        [Required]
        public string Title { get; set; }

        [Required]
        public string Instructions { get; set; }

        //list of tag ids
        public List<int> TagIds { get; set; }
    }
}
