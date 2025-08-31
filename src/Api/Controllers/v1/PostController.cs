using System;
using System.Collections.Generic;
using System.Linq;
using Api.Dtos.Comments;
using Api.Dtos.Posts;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Comments;
using Service.Posts;

namespace Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/posts")]
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
        public ActionResult<IEnumerable<PostResponse>> GetAll()
        {
            var posts = _postService.GetAll();

            var postsResponse = posts
                .Select(x => x.ToResponse());

            return Ok(postsResponse);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<PostResponse> Get([FromRoute] Guid id)
        {
            var post = _postService.Get(id);
            if (post == null)
            {
                return NotFound();
            }

            return Ok(post.ToResponse());
        }

        [HttpPost]
        public ActionResult<PostResponse> Post([FromBody] PostRequest request)
        {
            var domain = request.ToDomain();

            var createdPost = _postService.Create(domain);

            return CreatedAtAction(
                nameof(Get), 
                new
                { 
                    id = createdPost.Id
                }, 
                createdPost.ToResponse());
        }

        [HttpPut("{id:guid}")]
        public ActionResult<PostResponse> Put([FromRoute] Guid id, [FromBody] PostRequest request)
        {
            var domain = request.ToDomain(id);

            var updatedPost = _postService.Update(domain);

            if (updatedPost == null)
            {
                return NotFound();
            }

            return Ok(updatedPost.ToResponse());
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
        public ActionResult<IEnumerable<CommentResponse>> GetComments([FromRoute] Guid postId)
        {
            var comments = _commentService.GetByPostId(postId);

            var commentsResponse = comments
                .Select(x => x.ToResponse());

            return Ok(commentsResponse);
        }
    }
}