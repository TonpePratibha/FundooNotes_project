/*
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

       
        public NoteModel? GetNoteById(int id, int userId)
        {
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            return note != null ? ToModel(note) : null;
        }

      
        public void AddNote(NoteModel model)
        {
            var note = ToEntity(model);
            _context.Notes.Add(note);
            _context.SaveChanges();
        }

     
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





       

        





      
    }
}

*/
/*
using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories.Repo
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<NoteRepository> _logger;

        public NoteRepository(ApplicationDbContext context, JwtHelper jwtHelper, ILogger<NoteRepository> logger)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _logger = logger;
        }

        public IEnumerable<NoteModel> GetAllNotes(int userId)
        {
            _logger.LogInformation("Fetching all notes for UserId: {UserId}", userId);
            var notes = _context.Notes.Where(n => n.UserId == userId).ToList();
            return ToModelList(notes);
        }

        public NoteModel? GetNoteById(int id, int userId)
        {
            _logger.LogInformation("Fetching note with Id: {NoteId} for UserId: {UserId}", id, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            return note != null ? ToModel(note) : null;
        }

        public void AddNote(NoteModel model)
        {
            _logger.LogInformation("Adding a new note for UserId: {UserId}", model.UserId);
            var note = ToEntity(model);
            _context.Notes.Add(note);
            _context.SaveChanges();
            _logger.LogInformation("Note added successfully with Id: {NoteId}", note.Id);
        }

        public void UpdateNote(NoteModel model)
        {
            _logger.LogInformation("Updating note with Id: {NoteId} for UserId: {UserId}", model.Id, model.UserId);
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == model.Id && n.UserId == model.UserId);
            if (existingNote != null)
            {
                existingNote.Title = model.Title;
                existingNote.Description = model.Description;
                _context.SaveChanges();
                _logger.LogInformation("Note updated successfully with Id: {NoteId}", model.Id);
            }
            else
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for update", model.Id);
            }
        }

        public void DeleteNote(int id, int userId)
        {
            _logger.LogInformation("Deleting note with Id: {NoteId} for UserId: {UserId}", id, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();
                _logger.LogInformation("Note deleted successfully with Id: {NoteId}", id);
            }
            else
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for deletion", id);
            }
        }

        public NoteModel ToggleArchive(int noteId, int userId)
        {
            _logger.LogInformation("Toggling archive status for NoteId: {NoteId} and UserId: {UserId}", noteId, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null)
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for archiving", noteId);
                return null;
            }

            note.Archive = !note.Archive;
            if (note.Archive)
            {
                note.Trash = false;
            }

            _context.SaveChanges();
            _logger.LogInformation("Archive status toggled successfully for NoteId: {NoteId}", noteId);
            return ToModel(note);
        }

        public NoteModel ToggleTrash(int noteId, int userId)
        {
            _logger.LogInformation("Toggling trash status for NoteId: {NoteId} and UserId: {UserId}", noteId, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null)
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for trashing", noteId);
                return null;
            }

            if (!note.Trash)
            {
                note.Archive = false;
            }

            note.Trash = !note.Trash;
            _context.SaveChanges();
            _logger.LogInformation("Trash status toggled successfully for NoteId: {NoteId}", noteId);
            return ToModel(note);
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

        public static NoteModel ToModel(Note entity)
        {
            return new NoteModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UserId = entity.UserId,
                Archive = entity.Archive,
                Trash = entity.Trash,
            };
        }

        public static List<NoteModel> ToModelList(IEnumerable<Note> notes)
        {
            return notes.Select(ToModel).ToList();
        }
    }
}
*/


using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.JWT;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Repo
{
    public class NoteRepository : INoteRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtHelper _jwtHelper;
        private readonly ILogger<NoteRepository> _logger;
        private readonly IDistributedCache _cache;

        public NoteRepository(ApplicationDbContext context, JwtHelper jwtHelper, ILogger<NoteRepository> logger, IDistributedCache cache)
        {
            _context = context;
            _jwtHelper = jwtHelper;
            _logger = logger;
            _cache = cache;
        }

        // ✅ Get all notes with Redis caching
        public IEnumerable<NoteModel> GetAllNotes(int userId)
        {
            _logger.LogInformation("Fetching all notes for UserId: {UserId}", userId);

            string cacheKey = $"notes_{userId}";
            var cachedNotes = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedNotes))
            {
                _logger.LogInformation("Returning notes from cache for UserId: {UserId}", userId);
                return JsonSerializer.Deserialize<List<NoteModel>>(cachedNotes);
            }

            var notes = _context.Notes.Where(n => n.UserId == userId).ToList();
            var noteModels = ToModelList(notes);

            if (noteModels.Any())
            {
                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30) // Cache for 30 mins
                };
                _cache.SetString(cacheKey, JsonSerializer.Serialize(noteModels), cacheOptions);
            }

            return noteModels;
        }

        // ✅ Get note by ID with Redis caching
        public NoteModel? GetNoteById(int id, int userId)
        {
            _logger.LogInformation("Fetching note with Id: {NoteId} for UserId: {UserId}", id, userId);

            string cacheKey = $"note_{userId}_{id}";
            var cachedNote = _cache.GetString(cacheKey);
            if (!string.IsNullOrEmpty(cachedNote))
            {
                _logger.LogInformation("Returning note from cache for NoteId: {NoteId}", id);
                return JsonSerializer.Deserialize<NoteModel>(cachedNote);
            }

            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note == null) return null;

            var noteModel = ToModel(note);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            _cache.SetString(cacheKey, JsonSerializer.Serialize(noteModel), cacheOptions);

            return noteModel;
        }

        // ✅ Add note and clear cache
        public void AddNote(NoteModel model)
        {
            _logger.LogInformation("Adding a new note for UserId: {UserId}", model.UserId);
            var note = ToEntity(model);
            _context.Notes.Add(note);
            _context.SaveChanges();

            // Invalidate cache
            _cache.Remove($"notes_{model.UserId}");

            _logger.LogInformation("Note added successfully with Id: {NoteId}", note.Id);
        }

        // ✅ Update note and clear cache
        public void UpdateNote(NoteModel model)
        {
            _logger.LogInformation("Updating note with Id: {NoteId} for UserId: {UserId}", model.Id, model.UserId);
            var existingNote = _context.Notes.FirstOrDefault(n => n.Id == model.Id && n.UserId == model.UserId);
            if (existingNote != null)
            {
                existingNote.Title = model.Title;
                existingNote.Description = model.Description;
                _context.SaveChanges();

                // Invalidate cache
                _cache.Remove($"notes_{model.UserId}");
                _cache.Remove($"note_{model.UserId}_{model.Id}");

                _logger.LogInformation("Note updated successfully with Id: {NoteId}", model.Id);
            }
            else
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for update", model.Id);
            }
        }

        // ✅ Delete note and clear cache
        public void DeleteNote(int id, int userId)
        {
            _logger.LogInformation("Deleting note with Id: {NoteId} for UserId: {UserId}", id, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == id && n.UserId == userId);
            if (note != null)
            {
                _context.Notes.Remove(note);
                _context.SaveChanges();

                // Invalidate cache
                _cache.Remove($"notes_{userId}");
                _cache.Remove($"note_{userId}_{id}");

                _logger.LogInformation("Note deleted successfully with Id: {NoteId}", id);
            }
            else
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for deletion", id);
            }
        }


        public NoteModel ToggleArchive(int noteId, int userId)
        {
            _logger.LogInformation("Toggling archive status for NoteId: {NoteId} and UserId: {UserId}", noteId, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null)
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for archiving", noteId);
                return null;
            }

            note.Archive = !note.Archive;
            if (note.Archive)
            {
                note.Trash = false;
            }

            _context.SaveChanges();
            _logger.LogInformation("Archive status toggled successfully for NoteId: {NoteId}", noteId);
            return ToModel(note);
        }

        public NoteModel ToggleTrash(int noteId, int userId)
        {
            _logger.LogInformation("Toggling trash status for NoteId: {NoteId} and UserId: {UserId}", noteId, userId);
            var note = _context.Notes.FirstOrDefault(n => n.Id == noteId && n.UserId == userId);
            if (note == null)
            {
                _logger.LogWarning("Note with Id: {NoteId} not found for trashing", noteId);
                return null;
            }

            if (!note.Trash)
            {
                note.Archive = false;
            }

            note.Trash = !note.Trash;
            _context.SaveChanges();
            _logger.LogInformation("Trash status toggled successfully for NoteId: {NoteId}", noteId);
            return ToModel(note);
        }

        public static Note ToEntity(NoteModel model)
        {
            return new Note
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description,
                UserId = model.UserId,
                Archive = model.Archive,
                Trash = model.Trash
            };
        }

        public static NoteModel ToModel(Note entity)
        {
            return new NoteModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Description = entity.Description,
                UserId = entity.UserId,
                Archive = entity.Archive,
                Trash = entity.Trash
            };
        }

        public static List<NoteModel> ToModelList(IEnumerable<Note> notes)
        {
            return notes.Select(ToModel).ToList();
        }
    }
}
