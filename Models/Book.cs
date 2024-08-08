using System.ComponentModel.DataAnnotations;

namespace LibraryFinalsProject.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public string BookDescription { get; set; }
        public string PictureUrl { get; set; }

    }
}
