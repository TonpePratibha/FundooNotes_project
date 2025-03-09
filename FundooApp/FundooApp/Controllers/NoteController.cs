/*
    using BuisnessLayer.BL;
using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace FundooNotesApp.Controllers
{
    [Authorize]
    [Route("api/notes")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INotesBL _notesBL;
        private readonly JwtHelper _jwtHelper;
        public NoteController(INotesBL notesBL, JwtHelper jwtHelper)
        {
            _notesBL = notesBL;
            _jwtHelper = jwtHelper;
        }
       


        [HttpGet]

        public IActionResult GetNotes()
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            return Ok(_notesBL.GetAllNotes(userId));
        }


        [HttpGet("{id}")]

        public IActionResult GetNoteById(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            var note = _notesBL.GetNoteById(id, userId);

            if (note == null)
            {
                return NotFound(new { message = "Note not found." });
            }

            return Ok(note);
        }


        [HttpPost]

        public IActionResult Create(NoteModel model)
        {
            model.UserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            _notesBL.CreateNote(model);
            return Ok(new { message = "Note created successfully." });
        }

        [HttpPut("{id}")]

        public IActionResult Update(int id, NoteModel model)
        {
            model.UserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            model.Id = id;
            _notesBL.UpdateNote(model);
            return Ok(new { message = "Note updated successfully." });
        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
            _notesBL.DeleteNote(id, userId);
            return Ok(new { message = "Note deleted successfully." });
        }









        [HttpPut("archive/{id}")]
        [Authorize]
        public IActionResult ToggleArchive(int id)
        {
            // Extract user ID from JWT
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            var updatedNote = _notesBL.ToggleArchive(id, userId);

            if (updatedNote == null)
            {
                return NotFound(new { message = "Note not found or unauthorized." });
            }

            return Ok(new
            {
                message = updatedNote.Archive ? "Note archived successfully." : "Note unarchived successfully.",
                note = updatedNote
            });
        }


        [HttpPut("trash/{id}")]
        [Authorize]
        public IActionResult ToggleTrash(int id)
        {
            // Extract UserId from JWT
            var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");

            // Call Service Layer
            var updatedNote = _notesBL.ToggleTrash(id, userId);

            // Handle Not Found or Unauthorized
            if (updatedNote == null)
            {
                return NotFound(new { message = "Note not found or unauthorized." });
            }

            return Ok(new
            {
                message = updatedNote.Trash ? "Note moved to trash." : "Note restored from trash.",
                note = updatedNote
            });
        }

    }
}
*/


using BuisnessLayer.BL;
using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace FundooNotesApp.Controllers
{
    [Authorize]
    [Route("api/notes")]
    [ApiController]
    public class NoteController : ControllerBase
    {
        private readonly INotesBL _notesBL;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<NoteController> _logger;

        public NoteController(INotesBL notesBL, JwtHelper jwtHelper, ILogger<NoteController> logger)
        {
            _notesBL = notesBL;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetNotes()
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Fetching notes for UserId: {UserId}", userId);

                var notes = _notesBL.GetAllNotes(userId);
                return Ok(notes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching notes.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetNoteById(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Fetching note with Id: {Id} for UserId: {UserId}", id, userId);

                var note = _notesBL.GetNoteById(id, userId);
                if (note == null)
                {
                    _logger.LogWarning("Note not found: {Id}", id);
                    return NotFound(new { message = "Note not found." });
                }

                return Ok(note);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching note with Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPost]
        public IActionResult Create(NoteModel model)
        {
            try
            {
                model.UserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Creating a new note for UserId: {UserId}", model.UserId);

                _notesBL.CreateNote(model);
                return Ok(new { message = "Note created successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a note.");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, NoteModel model)
        {
            try
            {
                model.UserId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                model.Id = id;
                _logger.LogInformation("Updating note with Id: {Id} for UserId: {UserId}", id, model.UserId);

                _notesBL.UpdateNote(model);
                return Ok(new { message = "Note updated successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating note with Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Deleting note with Id: {Id} for UserId: {UserId}", id, userId);

                _notesBL.DeleteNote(id, userId);
                return Ok(new { message = "Note deleted successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting note with Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("archive/{id}")]
        public IActionResult ToggleArchive(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Toggling archive for note with Id: {Id} for UserId: {UserId}", id, userId);

                var updatedNote = _notesBL.ToggleArchive(id, userId);
                if (updatedNote == null)
                {
                    return NotFound(new { message = "Note not found or unauthorized." });
                }

                return Ok(new
                {
                    message = updatedNote.Archive ? "Note archived successfully." : "Note unarchived successfully.",
                    note = updatedNote
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling archive status for note with Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }

        [HttpPut("trash/{id}")]
        public IActionResult ToggleTrash(int id)
        {
            try
            {
                var userId = int.Parse(User.FindFirst("userId")?.Value ?? "0");
                _logger.LogInformation("Toggling trash for note with Id: {Id} for UserId: {UserId}", id, userId);

                var updatedNote = _notesBL.ToggleTrash(id, userId);
                if (updatedNote == null)
                {
                    return NotFound(new { message = "Note not found or unauthorized." });
                }

                return Ok(new
                {
                    message = updatedNote.Trash ? "Note moved to trash." : "Note restored from trash.",
                    note = updatedNote
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while toggling trash status for note with Id: {Id}", id);
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}
