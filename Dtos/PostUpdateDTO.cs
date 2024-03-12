namespace DotnetAPI.Dtos
{
    public class PostUpdateDTO
    {
        public int PostId { get; set; }
        public string PostTitle { get; set; } = "";
        public string PostContent { get; set; } = "";
    }
}