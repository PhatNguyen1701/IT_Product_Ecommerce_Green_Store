using ITProductECommerce.Data;
using System.ComponentModel.DataAnnotations;

namespace ITProductECommerce.ViewModels
{
    public class CommentVM
    {
        [Required]
        public int MainCommentId { get; set; }

        [Required]
        public string Message { get; set; } = "";

        public DateTime Created { get; set; }

        public string CustomerId { get; set; }

        public string CustomerName { get; set; }

        public int Rating { get; set; } = 5;

        [Required]
        public int ProductId { get; set; }

        public string? ProductName { get; set; }
        public string? ImageURL { get; set; }
    }
}
