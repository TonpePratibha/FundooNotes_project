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
        /*
                [HttpPost]
                public IActionResult CreateNote([FromBody] Note note)
                {
                    var userId = _notesBL.GetUserIdFromToken(User);
                    note.UserId = userId;

                    var createdNote = _notesBL.CreateNote(note);
                    return Ok(createdNote);
                }

                [HttpGet]
                public IActionResult GetNotes()
                {
                    var userId = _notesBL.GetUserIdFromToken(User);
                    var notes = _notesBL.GetNotesByUserId(userId);
                    return Ok(notes);
                }

                [HttpGet("{id}")]
                public IActionResult GetNoteById(int id)
                {
                    var userId = _notesBL.GetUserIdFromToken(User);
                    var note = _notesBL.GetNoteById(id, userId);
                    if (note == null) return NotFound();
                    return Ok(note);
                }

                [HttpPut("{id}")]
                public IActionResult UpdateNote(int id, [FromBody] Note note)
                {
                    var userId = _notesBL.GetUserIdFromToken(User);
                    var existingNote = _notesBL.GetNoteById(id, userId);
                    if (existingNote == null) return NotFound();

                    existingNote.Title = note.Title;
                    existingNote.Description = note.Description;
                    existingNote.Color = note.Color;

                    var updatedNote = _notesBL.UpdateNote(existingNote);
                    return Ok(updatedNote);
                }

                [HttpDelete("{id}")]
                public IActionResult DeleteNote(int id)
                {
                    var userId = _notesBL.GetUserIdFromToken(User);
                    var success = _notesBL.DeleteNote(id, userId);
                    if (!success) return NotFound();
                    return NoContent();
                }
        */




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
