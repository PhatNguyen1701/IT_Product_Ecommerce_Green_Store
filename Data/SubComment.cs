namespace ITProductECommerce.Data
{
    public class SubComment
    {
        public int SubCommentId { get; set; }
        public string Message { get; set; } = "";
        public DateTime Created { get; set; }
        public int MainCommentId { get; set; }
        public string UserId { get; set; }

        public MainComment MainComment { get; set; }
        public User User { get; set; }
    }
}
