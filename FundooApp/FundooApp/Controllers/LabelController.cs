/*
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
*/

using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;

namespace FundooNotesApp.Controllers
{
    [Authorize]
    [Route("api/labels")]
    [ApiController]
    public class LabelController : ControllerBase
    {
        private readonly ILabelBL _labelBL;
        private readonly ILogger<LabelController> _logger;

        public LabelController(ILabelBL labelBL, ILogger<LabelController> logger)
        {
            _labelBL = labelBL;
            _logger = logger;
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst("userId")?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                _logger.LogError("User ID not found in token.");
                throw new InvalidOperationException("User ID not found in token.");
            }

            return int.Parse(userIdClaim);
        }

        [HttpGet]
        public IActionResult GetLabels()
        {
            try
            {
                int userId = GetUserId();
                _logger.LogInformation($"Fetching labels for user ID: {userId}");

                var labels = _labelBL.GetLabelsByUser(userId);
                return Ok(labels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching labels.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{noteId}")]
        public IActionResult GetLabelsByNote(int noteId)
        {
            try
            {
                _logger.LogInformation($"Fetching labels for note ID: {noteId}");

                var labels = _labelBL.GetLabelsByNote(noteId);
                return Ok(labels);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while fetching labels by note.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("create")]
        public IActionResult CreateLabel([FromBody] Label label)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid label model received.");
                    return BadRequest(ModelState);
                }

                _labelBL.CreateLabel(label);
                _logger.LogInformation($"Label created successfully: {label}");

                return Ok(new { message = "Label created successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating label.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost("link")]
        public IActionResult LinkLabelToNote([FromQuery] int noteId, [FromQuery] int labelId)
        {
            try
            {
                _labelBL.LinkLabelToNote(noteId, labelId);
                _logger.LogInformation($"Linked label {labelId} to note {noteId}");

                return Ok(new { message = "Label linked to note successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while linking label to note.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateLabel(int id, [FromBody] Label label)
        {
            try
            {
                if (id != label.id)
                {
                    _logger.LogWarning("Label ID mismatch.");
                    return BadRequest("Label ID mismatch");
                }

                _labelBL.UpdateLabel(label);
                _logger.LogInformation($"Label updated successfully: {label}");

                return Ok(new { message = "Label updated successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while updating label.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteLabel(int id)
        {
            try
            {
                _labelBL.DeleteLabel(id);
                _logger.LogInformation($"Label deleted successfully. Label ID: {id}");

                return Ok(new { message = "Label deleted successfully" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while deleting label.");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
