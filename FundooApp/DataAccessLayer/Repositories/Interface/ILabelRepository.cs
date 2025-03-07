using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories.Interface
{
   public interface ILabelRepository
    {
        public IEnumerable<DataAccessLayer.Entities.Label> GetLabelsByUserId(int userId);
        public IEnumerable<DataAccessLayer.Entities.Label> GetLabelsByNoteId(int noteId);
        public void AddLabel(DataAccessLayer.Entities.Label label);
        public void AddLabelToNote(int noteId, int labelId);
        public void UpdateLabel(DataAccessLayer.Entities.Label label);
        public void DeleteLabel(int labelId);
    }
}
