using Api.Dtos.Comments;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Service.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Controllers.v1
{
    [ApiController]
    [Route("api/v1/comments")]
    public class CommentController : ControllerBase
    {
        private readonly ILogger<CommentController> _logger;
        private readonly ICommentService _commentService;

        public CommentController(
            ILogger<CommentController> logger,
            ICommentService commentService)
        {
            _logger = logger;
            _commentService = commentService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentResponse>>> GetAll()
        {
            var comments = await _commentService.GetAllAsync();

            var commentsResponse = comments
                .Select(x => x.ToResponse());

            return Ok(commentsResponse);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CommentResponse>> Get([FromRoute] Guid id)
        {
            var comment = await _commentService.GetAsync(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToResponse());
        }

        [HttpPost]
        public async Task<ActionResult<CommentResponse>> Post([FromBody] CommentRequest request)
        {
            var domain = request.ToDomain();

            var createdComment = await _commentService.CreateAsync(domain);

            return CreatedAtAction(
                nameof(Get),
                new 
                { 
                    id = createdComment.Id 
                },
                createdComment.ToResponse());
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CommentResponse>> Put([FromRoute] Guid id, [FromBody] CommentRequest request)
        {
            var domain = request.ToDomain(id);

            var updatedComment = await _commentService.UpdateAsync(domain);

            if (updatedComment == null)
            {
                return NotFound();
            }

            return Ok(updatedComment.ToResponse());
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deleted = await _commentService.DeleteAsync(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}