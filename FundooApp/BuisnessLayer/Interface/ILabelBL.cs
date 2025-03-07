using DataAccessLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.Interface
{
  public interface ILabelBL
    {

        public IEnumerable<Label> GetLabelsByUser(int userId);
        public IEnumerable<Label> GetLabelsByNote(int noteId);
        public void CreateLabel(Label label);
        public void LinkLabelToNote(int noteId, int labelId);
        public void UpdateLabel(Label label);
        public void DeleteLabel(int id);

    }
}
