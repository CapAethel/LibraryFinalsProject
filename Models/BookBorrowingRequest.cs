namespace LibraryFinalsProject.Models
{
    public class BookBorrowingRequest
    {
        public int Id { get; set; }
        public User Requestor { get; set; }
        public DateTime DateRequested { get; set; }
        public string Status { get; set; }
        public User Approver { get; set; }
        public ICollection<BookBorrowingRequestDetails> BookBorrowingRequestDetails { get; set; }
    }
}
