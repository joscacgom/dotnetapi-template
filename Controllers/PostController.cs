using DotnetAPI.Data;
using DotnetAPI.Dtos;
using DotnetAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DotnetAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class PostController : ControllerBase
    {
        private readonly DataContextDapper _context;

        public PostController(IConfiguration configuration)
        {
            _context = new DataContextDapper(configuration);

        }

        [HttpGet("GetAllPosts")]
        public IEnumerable<Post> GetAllPosts()
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts";
            return _context.LoadData<Post>(sql);
        }

        [HttpGet("GetPostById")]
        public Post? GetPostById(int id)
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts WHERE PostId = " + id.ToString();
            return _context.LoadDataSingle<Post>(sql);
        }

        [HttpGet("GetPostByUserId/{userId}")]
        public IEnumerable<Post> GetPostByUserId(int userId)
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts WHERE UserId = " + userId.ToString();
            return _context.LoadData<Post>(sql);
        }

        [HttpGet("GetMyPosts")]
        public IEnumerable<Post> GetMyPosts()
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts WHERE UserId = " + User.FindFirst("userId")?.Value;
            return _context.LoadData<Post>(sql);
        }

        [HttpPost("AddPost")]
        public IActionResult AddPost(PostAddDTO post)
        {
            string sql = @"
                INSERT INTO TutorialAppSchema.Posts (UserId, PostTitle, PostContent, CreatedAt, UpdatedAt)
                VALUES (" + User.FindFirst("userId")?.Value + " , @PostTitle, @PostContent ,GETDATE(), GETDATE())";
            if (_context.ExecuteData(sql, post))
            {
                return Ok();
            }
            return BadRequest();

        }

        [HttpPut("UpdatePost")]
        public IActionResult UpdatePost(PostUpdateDTO post)
        {
            string sql = @"
                UPDATE TutorialAppSchema.Posts
                SET PostTitle = @PostTitle, PostContent = @PostContent, UpdatedAt = GETDATE()
                WHERE PostId = @PostId" + " AND UserId = " + User.FindFirst("userId")?.Value;
            if (_context.ExecuteData(sql, post))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpDelete("DeletePost/{postId}")]
        public IActionResult DeletePost(int postId)
        {
            string sql = "DELETE FROM TutorialAppSchema.Posts WHERE PostId = @PostId AND UserId = " + User.FindFirst("userId")?.Value;
            if (_context.ExecuteData(sql, new { PostId = postId }))
            {
                return Ok();
            }
            return BadRequest();
        }

        [HttpGet("SearchPost/{searchParam}")]
        public IEnumerable<Post> SearchPost(string searchParam)
        {
            string sql = "SELECT * FROM TutorialAppSchema.Posts WHERE PostTitle LIKE '%" + searchParam + "%' OR PostContent LIKE '%" + searchParam + "%'";
            return _context.LoadData<Post>(sql);
        }

    }
}