namespace LibraryFinalsProject.ViewModels
{
    public class BookViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string? CategoryName { get; set; }
        public int CategoryId { get; set; }
        public string? BookDescription { get; set; }  // Add Description
        public string? PictureUrl { get; set; }   // Add PictureUrl
    }
}
