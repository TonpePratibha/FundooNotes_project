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

        /*
                public Note CreateNote(Note note) => _noteRepository.CreateNote(note);

                public IEnumerable<Note> GetNotesByUserId(int userId) => _noteRepository.GetNotesByUserId(userId);

                public Note GetNoteById(int id, int userId) => _noteRepository.GetNoteById(id, userId);

                public Note UpdateNote(Note note) => _noteRepository.UpdateNote(note);

                public bool DeleteNote(int id, int userId) => _noteRepository.DeleteNote(id, userId);

                public int GetUserIdFromToken(System.Security.Claims.ClaimsPrincipal user)
                    => _noteRepository.GetUserIdFromToken(user);
        */





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
