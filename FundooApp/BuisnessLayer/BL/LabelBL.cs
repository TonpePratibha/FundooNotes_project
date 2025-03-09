/*
using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using DataAccessLayer.Repositories.Repo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BuisnessLayer.BL
{
  public class LabelBL:ILabelBL
        
    {
       
        private readonly ILabelRepository _labelRepository;

        public LabelBL(ILabelRepository labelRepository)
        {
            _labelRepository = labelRepository;
        }

        // Get all labels for a user
        public IEnumerable<Label> GetLabelsByUser(int userId)
        {
            return _labelRepository.GetLabelsByUserId(userId);
        }

        // Get all labels linked to a note
        public IEnumerable<Label> GetLabelsByNote(int noteId)
        {
            return _labelRepository.GetLabelsByNoteId(noteId);
        }

        // Create a new label
        public void CreateLabel(Label label)
        {
            _labelRepository.AddLabel(label);
        }

        // Associate a label with a note
        public void LinkLabelToNote(int noteId, int labelId)
        {
            _labelRepository.AddLabelToNote(noteId, labelId);
        }

        // Update a label
        public void UpdateLabel(Label label)
        {
            _labelRepository.UpdateLabel(label);
        }

        // Delete a label
        public void DeleteLabel(int id)
        {
            _labelRepository.DeleteLabel(id);
        }

    }
}
*/


using BuisnessLayer.Interface;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace BuisnessLayer.BL
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRepository _labelRepository;
        private readonly ILogger<LabelBL> _logger;

        public LabelBL(ILabelRepository labelRepository, ILogger<LabelBL> logger)
        {
            _labelRepository = labelRepository;
            _logger = logger;
        }

        public IEnumerable<Label> GetLabelsByUser(int userId)
        {
            _logger.LogInformation($"Fetching labels for user ID: {userId}");
            return _labelRepository.GetLabelsByUserId(userId);
        }

        public IEnumerable<Label> GetLabelsByNote(int noteId)
        {
            _logger.LogInformation($"Fetching labels for note ID: {noteId}");
            return _labelRepository.GetLabelsByNoteId(noteId);
        }

        public void CreateLabel(Label label)
        {
            _logger.LogInformation($"Creating label: {label}");
            _labelRepository.AddLabel(label);
        }

        public void LinkLabelToNote(int noteId, int labelId)
        {
            _logger.LogInformation($"Linking label {labelId} to note {noteId}");
            _labelRepository.AddLabelToNote(noteId, labelId);
        }

        public void UpdateLabel(Label label)
        {
            _logger.LogInformation($"Updating label: {label}");
            _labelRepository.UpdateLabel(label);
        }

        public void DeleteLabel(int id)
        {
            _logger.LogInformation($"Deleting label with ID: {id}");
            _labelRepository.DeleteLabel(id);
        }
    }
}
