using System.ComponentModel.DataAnnotations;

namespace Library.Models.Entities
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Title { get; set; }
        [Required]
        [StringLength(100)]
        public string Author { get; set; }
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public bool Available{ get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
