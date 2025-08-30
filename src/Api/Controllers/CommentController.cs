using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Model.Comments;
using Service.Comments;
using System;
using System.Collections.Generic;

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
        public ActionResult<IEnumerable<Comment>> GetAll()
        {
            var comments = _commentService.GetAll();

            return Ok(comments);
        }

        [HttpGet("{id:guid}")]
        public ActionResult<Comment> Get([FromRoute] Guid id)
        {
            var comment = _commentService.Get(id);

            if (comment == null)
            {
                return NotFound();
            }

            return Ok(comment);
        }

        [HttpPost]
        public ActionResult<Comment> Post([FromBody] Comment comment)
        {
            var createdComment = _commentService.Create(comment);

            return CreatedAtAction(
                nameof(Get),
                new 
                { 
                    id = createdComment.Id 
                },
                createdComment);
        }

        [HttpPut("{id:guid}")]
        public IActionResult Put([FromRoute] Guid id, [FromBody] Comment comment)
        {
            if (id != comment.Id)
            {
                return BadRequest("Invalid Id.");
            }

            var updatedComment = _commentService.Update(comment);

            if (updatedComment == null)
            {
                return NotFound();
            }

            return Ok(updatedComment);
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