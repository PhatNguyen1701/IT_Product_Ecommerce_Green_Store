namespace ITProductECommerce.Data
{
    public class SubComment
    {
        public int SubCommentId { get; set; }
        public string Message { get; set; } = "";
        public DateTime Created { get; set; }
        public int MainCommentId { get; set; }
        public string CustomerId { get; set; }

        public MainComment MainComment { get; set; }
        public Customer Customer { get; set; }
    }
}
