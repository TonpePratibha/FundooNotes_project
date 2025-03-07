using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundooNotesApp.Controllers
{

    [Authorize]
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {


        private readonly ILabelBL _labelBL;
        
        public LabelController(ILabelBL labelBL) { 
            _labelBL = labelBL;
        }


        [HttpGet]
        //  private int GetUserId() => int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                throw new InvalidOperationException("User ID not found in token.");
            }

            return int.Parse(userIdClaim);
        }

        // Get all labels for the logged-in user
        [HttpGet]
        public IActionResult GetLabels()
        {
            int userId = GetUserId();
            var labels = _labelBL.GetLabelsByUser(userId);
            return Ok(labels);
        }

        // Get all labels for a specific note
        [HttpGet("{noteId}")]
        public IActionResult GetLabelsByNote(int noteId)
        {
            var labels = _labelBL.GetLabelsByNote(noteId);
            return Ok(labels);
        }

        // Create a new label
        [HttpPost("create")]
        public IActionResult CreateLabel([FromBody] Label label)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            _labelBL.CreateLabel(label);
            return Ok(new { message = "Label created successfully" });
        }

        // Associate an existing label with a note
        [HttpPost("link")]
        public IActionResult LinkLabelToNote([FromQuery] int noteId, [FromQuery] int labelId)
        {
            _labelBL.LinkLabelToNote(noteId, labelId);
            return Ok(new { message = "Label linked to note successfully" });
        }

        // Update a label
        [HttpPut("{id}")]
        public IActionResult UpdateLabel(int id, [FromBody] Label label)
        {
            if (id != label.id) return BadRequest("Label ID mismatch");
            _labelBL.UpdateLabel(label);
            return Ok(new { message = "Label updated successfully" });
        }

        // Delete a label and its associations
        [HttpDelete("{id}")]
        public IActionResult DeleteLabel(int id)
        {
            _labelBL.DeleteLabel(id);
            return Ok(new { message = "Label deleted successfully" });
        }
    }
}
