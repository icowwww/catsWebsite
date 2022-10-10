namespace KittenApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Kitten
    {
        public Kitten()
        {
            this.Comments = new List<Comment>();
        }
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }

        [Required]
        [Range(0, 20)]
        public int Age { get; set; }

        public int BreedId { get; set; }

        public Breed Breed { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
