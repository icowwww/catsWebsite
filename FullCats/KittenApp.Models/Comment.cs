namespace KittenApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(35)]
        public string CommentContent { get; set; }

        public int KittenId { get; set; }

        public Kitten Kitten { get; set; }

        public int UserId { get; set; }

        public User User { get; set; } 
    }
}
