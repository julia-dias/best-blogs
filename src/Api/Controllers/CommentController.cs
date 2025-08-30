using Api.Dtos.Comments;
using Api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Service.Comments;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Api.Controllers
{
    [ApiController]
    [Route("comments")]
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
        public ActionResult<IEnumerable<CommentResponse>> GetAll()
        {
            var comments = _commentService.GetAll();

            var commentsResponse = comments
                .Select(x => x.ToResponse());

            return Ok(commentsResponse);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<CommentResponse> Get([FromRoute] Guid id)
        {
            var comment = _commentService.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment.ToResponse());
        }

        [HttpPost]
        public ActionResult<CommentResponse> Post([FromBody] CommentRequest request)
        {
            var domain = request.ToDomain();

            var createdComment = _commentService.Create(domain);

            return CreatedAtAction(
                nameof(Get),
                new 
                { 
                    id = createdComment.Id 
                },
                createdComment.ToResponse());
        }

        [HttpPut("{id:guid}")]
        public ActionResult<CommentResponse> Put([FromRoute] Guid id, [FromBody] CommentRequest request)
        {
            var domain = request.ToDomain(id);

            var updatedComment = _commentService.Update(domain);

            if (updatedComment == null)
            {
                return NotFound();
            }

            return Ok(updatedComment.ToResponse());
        }

        [HttpDelete("{id:guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var deleted = _commentService.Delete(id);

            if (!deleted)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}