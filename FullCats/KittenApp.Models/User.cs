namespace KittenApp.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Comments = new List<Comment>();
        }
        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
