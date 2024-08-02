namespace LibraryFinalsProject.Models
{
    public class BookBorrowingRequestDetails
    {
        public int Id { get; set; }
        public BookBorrowingRequest BookBorrowingRequest { get; set; }
        public Book Book { get; set; }
    }
}
