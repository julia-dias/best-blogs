using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Model.Posts;
using Service.Comments;
using Service.Posts;

namespace Api.Controllers
{
    [ApiController]
    [Route("posts")]
    public class PostController : ControllerBase
    {
        private readonly ILogger<PostController> _logger;
        private readonly IPostService _postService;
        private readonly ICommentService _commentService;

        public PostController(
            ILogger<PostController> logger,
            IPostService postService,
            ICommentService commentService)
        {
            _logger = logger;
            _postService = postService;
            _commentService = commentService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Post>> GetAll()
        {
            var posts = _postService.GetAll();
            return Ok(posts);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Post> Get([FromRoute] Guid id)
        {
            var post = _postService.Get(id);
            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public ActionResult<Post> Post([FromBody] Post post)
        {
            var createdPost = _postService.Create(post);
            return CreatedAtAction(
                nameof(Get), 
                new
                { 
                    id = createdPost.Id
                }, 
                createdPost);
        }

        [HttpPut("{id:guid}")]
        public ActionResult<Post> Put([FromRoute] Guid id, [FromBody] Post post)
        {
            if (id != post.Id)
            {
                return BadRequest("Invalid Id.");
            }

            var updatedPost = _postService.Update(post);

            if (updatedPost == null)
            {
                return NotFound();
            }

            return Ok(updatedPost);
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var deleted = _postService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpGet("{id:guid}/comments")]
        public ActionResult<IEnumerable<Comment>> GetComments([FromRoute] Guid id)
        {
            var comments = _commentService.GetByPostId(id);

            return Ok(comments);
        }
    }
}