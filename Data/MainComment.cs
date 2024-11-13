namespace ITProductECommerce.Data
{
    public class MainComment
    {
        public int MainCommentId { get; set; }
        public string Message { get; set; } = "";
        public DateTime Created { get; set; }
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int Rating { get; set; }

        public User User { get; set; }
        public Product Product { get; set; }
        public ICollection<SubComment> SubComments { get; set; }

        public MainComment()
        {
            SubComments = new List<SubComment>();
        }
    }
}
