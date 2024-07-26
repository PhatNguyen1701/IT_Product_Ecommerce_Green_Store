namespace ITProductECommerce.Data
{
    public class MainComment
    {
        public int MainCommentId { get; set; }
        public string Message { get; set; } = "";
        public DateTime Created { get; set; }
        public string CustomerId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
        public ICollection<SubComment> SubComments { get; set; }

        public MainComment()
        {
            SubComments = new List<SubComment>();
        }
    }
}
