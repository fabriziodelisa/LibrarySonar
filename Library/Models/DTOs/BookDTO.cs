namespace Library.Models.DTOs
{
    public class BookDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? Publisher { get; set; }
        public int? PublicationYear { get; set; }
        public string Category { get; set; }
        public string Location { get; set; }
        public bool Available { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
