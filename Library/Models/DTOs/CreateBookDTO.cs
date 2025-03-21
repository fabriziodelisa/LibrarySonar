
namespace Library.Models.DTOs
{
    public class CreateBookDTO
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
    }
}
