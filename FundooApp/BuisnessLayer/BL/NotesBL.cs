/*
using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.BL
{
    public class NotesBL:INotesBL
    {
        private readonly INoteRepository _noteRepository;

        public NotesBL(INoteRepository noterepository) { 
            _noteRepository= noterepository;
        }

       


        public IEnumerable<NoteModel> GetAllNotes(int userId) => _noteRepository.GetAllNotes(userId);

        public NoteModel? GetNoteById(int id, int userId) => _noteRepository.GetNoteById(id, userId);

        public void CreateNote(NoteModel model) => _noteRepository.AddNote(model);

        public void UpdateNote(NoteModel model) => _noteRepository.UpdateNote(model);

        public void DeleteNote(int id, int userId) => _noteRepository.DeleteNote(id, userId);
        public NoteModel? ToggleArchive(int noteId, int userId)
        {
            // Call the repository method directly
            return _noteRepository.ToggleArchive(noteId, userId);
        }
        public NoteModel ToggleTrash(int noteId, int userId)
        {
            return _noteRepository.ToggleTrash(noteId, userId);
        }

    }
}
*/


using BuisnessLayer.Interface;
using DataAccessLayer.Models;
using DataAccessLayer.Repositories.Interface;
using Microsoft.Extensions.Logging;

namespace BuisnessLayer.BL
{
    public class NotesBL : INotesBL
    {
        private readonly INoteRepository _noteRepository;
        private readonly ILogger<NotesBL> _logger;

        public NotesBL(INoteRepository noteRepository, ILogger<NotesBL> logger)
        {
            _noteRepository = noteRepository;
            _logger = logger;
        }

        public IEnumerable<NoteModel> GetAllNotes(int userId)
        {
            _logger.LogInformation("Getting all notes for user {UserId}", userId);
            return _noteRepository.GetAllNotes(userId);
        }

        public NoteModel? GetNoteById(int id, int userId)
        {
            _logger.LogInformation("Getting note with ID {NoteId} for user {UserId}", id, userId);
            return _noteRepository.GetNoteById(id, userId);
        }

        public void CreateNote(NoteModel model)
        {
            _logger.LogInformation("Creating a new note for user {UserId}", model.UserId);
            _noteRepository.AddNote(model);
        }

        public void UpdateNote(NoteModel model)
        {
            _logger.LogInformation("Updating note with ID {NoteId} for user {UserId}", model.Id, model.UserId);
            _noteRepository.UpdateNote(model);
        }

        public void DeleteNote(int id, int userId)
        {
            _logger.LogInformation("Deleting note with ID {NoteId} for user {UserId}", id, userId);
            _noteRepository.DeleteNote(id, userId);
        }

        public NoteModel? ToggleArchive(int noteId, int userId)
        {
            _logger.LogInformation("Toggling archive status for note {NoteId} by user {UserId}", noteId, userId);
            return _noteRepository.ToggleArchive(noteId, userId);
        }

        public NoteModel ToggleTrash(int noteId, int userId)
        {
            _logger.LogInformation("Toggling trash status for note {NoteId} by user {UserId}", noteId, userId);
            return _noteRepository.ToggleTrash(noteId, userId);
        }
    }
}
