namespace DotnetAPI.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public int UserId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}