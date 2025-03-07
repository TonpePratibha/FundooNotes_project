using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Repo
{
    public class NoteRepository:INoteRepository
    {
        private ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;

        public NoteRepository(ApplicationDbContext context, JwtHelper jwtHelper) {
           _context = context;
            _jwtHelper = jwtHelper;
        }
        public IEnumerable<NoteModel> GetAllNotes(int userId)
        {
            var notes = _context.Notes.Where(n => n.UserId == userId).ToList();
            return ToModelList(notes);
        }

        // ✅ Get a Note by ID (Mapped to NoteModel)
        public NoteModel? GetNoteById(int id, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            return note != null ? ToModel(note) : null;
        }

        // ✅ Add a New Note (Using NoteModel)
        public void AddNote(NoteModel model)
        {
            var note = ToEntity(model);
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

        // ✅ Update an Existing Note
        public void UpdateNote(NoteModel model)
        {
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == model.Id && n.UserId == model.UserId);
            if (existingNote != null)
            {
                existingNote.Title = model.Title;
                existingNote.Description = model.Description;
                _context.SaveChanges();
            }
        }

        // ✅ Delete a Note by ID
        public void DeleteNote(int id, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
            }
        }

        public static Note ToEntity(NoteModel model)
        {
            return new Note
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId
            };
        }

        // ✅ Map from Note Entity to NoteModel
        public static NoteModel ToModel(Note entity)
        {
            return new NoteModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UserId = entity.UserId,
                Archive =entity.Archive, 
                Trash = entity.Trash,
            };
        }

        
        public static List<NoteModel> ToModelList(IEnumerable<Note> notes)
        {
            return notes.Select(ToModel).ToList();
        }




        // Toggle Archive Status (Can Archive or Unarchive)
        public NoteModel ToggleArchive(int noteId, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null) return null;

            note.Archive = !note.Archive;

            // If Archiving, Ensure it's NOT Trashed
            if (note.Archive)
            {
                note.Trash = false;
            }

            _context.SaveChanges();
            return ToModel(note);
        }

        // Toggle Trash Status (Cannot Archive if Trashed)
        public NoteModel ToggleTrash(int noteId, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null) return null;

            // If the note is being trashed, set archive to false
            if (!note.Trash)
            {
                note.Archive = false; // Ensure Trashed Notes cannot be Archived
            }

            note.Trash = !note.Trash;

            _context.SaveChanges();
            return ToModel(note);
        }



        /*
        public NoteModel? ToggleArchive(int noteId, int userId)
        {
            // Fetch the note belonging to the user
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null) return null;

            // Toggle archive status
            note.Archive = !note.Archive;

            // Update note and save changes
            _context.Notes.Update(note);
            _context.SaveChanges();

            // Map the entity to the model
            return ToModel(note);
        }


       

        // Toggle Trash Status - Ensure Archived is False
        public NoteModel ToggleTrash(int noteId, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null) return null;

            note.Trash = !note.Trash;

            // Ensure note cannot be both archived and trashed
            if (note.Trash)
            {
                note.Archive = false;
            }

            _context.SaveChanges();

            // Return as NoteModel
            return ToModel(note);
        }



        */





        /*
        public Note CreateNote(Note note)
        {
            _context.Notes.Add(note);
            _context.SaveChanges();
            return note;
        }

        public IEnumerable<Note> GetNotesByUserId(int userId)
        {
            return _context.Notes.Where(n => n.UserId == userId).ToList();
        }

        public Note GetNoteById(int id, int userId)
        {
            return _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
        }

        public Note UpdateNote(Note note)
        {
            _context.Notes.Update(note);
            _context.SaveChanges();
            return note;
        }

        public bool DeleteNote(int id, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return false;

            _context.Notes.Remove(note);
            _context.SaveChanges();
            return true;
        }

        public int GetUserIdFromToken(System.Security.Claims.ClaimsPrincipal user)
        {
            return _jwtHelper.GetUserIdFromToken(user);
        }
        */
    }
}
