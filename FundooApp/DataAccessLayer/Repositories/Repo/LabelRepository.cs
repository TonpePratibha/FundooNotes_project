using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Repo
{
    public class LabelRepository:ILabelRepository
    {

        private readonly ApplicationDbContext _context;
        public LabelRepository(ApplicationDbContext context) {
        _context = context;
        }

        public IEnumerable<Label> GetLabelsByUserId(int userId)
        {
            return _context.Labels
                .Where(label => label.NoteLabels.Any(nl => nl.note.UserId == userId))
                .Include(label => label.NoteLabels)
                .ToList();
        }

        // Get labels by a specific note
        public IEnumerable<Label> GetLabelsByNoteId(int noteId)
        {
            return _context.NoteLabels
                .Where(nl => nl.NoteId == noteId)
                .Select(nl => nl.label)
                .ToList();
        }

        // Create a new label
        public void AddLabel(Label label)
        {
            _context.Labels.Add(label);
            _context.SaveChanges();
        }

        // Associate a label with a note
        public void AddLabelToNote(int noteId, int labelId)
        {
            if (!_context.NoteLabels.Any(nl => nl.NoteId == noteId && nl.LabelId == labelId))
            {
                _context.NoteLabels.Add(new NoteLabel { NoteId = noteId, LabelId = labelId });
                _context.SaveChanges();
            }
        }

        // Update an existing label
        public void UpdateLabel(Label label)
        {
            _context.Labels.Update(label);
            _context.SaveChanges();
        }

        // Delete label and its associations
        public void DeleteLabel(int labelId)
        {
            var label = _context.Labels.FirstOrDefault(l => l.id == labelId);
            if (label != null)
            {
                _context.NoteLabels.RemoveRange(_context.NoteLabels.Where(nl => nl.LabelId == labelId));
                _context.Labels.Remove(label);
                _context.SaveChanges();
            }
        }

    }
}
