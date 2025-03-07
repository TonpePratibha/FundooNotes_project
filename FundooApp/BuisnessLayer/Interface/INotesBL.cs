using DataAccessLayer.Entities;
using DataAccessLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Interface
{
    public  interface INotesBL
    {
        /*
        public Note CreateNote(Note note);
        public IEnumerable<Note> GetNotesByUserId(int userId);
        public Note GetNoteById(int id, int userId);
        public Note UpdateNote(Note note);
        public bool DeleteNote(int id, int userId);
        public int GetUserIdFromToken(System.Security.Claims.ClaimsPrincipal user);
        */



        public IEnumerable<NoteModel> GetAllNotes(int userId) ;

        public NoteModel? GetNoteById(int id, int userId);

        public void CreateNote(NoteModel model);

        public void UpdateNote(NoteModel model);

        public void DeleteNote(int id, int userId);
        NoteModel? ToggleArchive(int noteId, int userId);
        public NoteModel ToggleTrash(int noteId, int userId);
    }
}
