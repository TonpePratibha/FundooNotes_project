/*
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
*/
/*
using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.Repositories.Repo
{
    public class LabelRepository : ILabelRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LabelRepository> _logger;

        public LabelRepository(ApplicationDbContext context, ILogger<LabelRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IEnumerable<Label> GetLabelsByUserId(int userId)
        {
            _logger.LogInformation($"Fetching labels for user ID: {userId}");

            return _context.Labels
                .Where(label => label.NoteLabels.Any(nl => nl.note.UserId == userId))
                .Include(label => label.NoteLabels)
                .ToList();
        }

        public IEnumerable<Label> GetLabelsByNoteId(int noteId)
        {
            _logger.LogInformation($"Fetching labels for note ID: {noteId}");

            return _context.NoteLabels
                .Where(nl => nl.NoteId == noteId)
                .Select(nl => nl.label)
                .ToList();
        }

        public void AddLabel(Label label)
        {
            _logger.LogInformation($"Adding label: {label}");

            _context.Labels.Add(label);
            _context.SaveChanges();
        }

        public void AddLabelToNote(int noteId, int labelId)
        {
            if (!_context.NoteLabels.Any(nl => nl.NoteId == noteId && nl.LabelId == labelId))
            {
                _context.NoteLabels.Add(new NoteLabel { NoteId = noteId, LabelId = labelId });
                _logger.LogInformation($"Added label {labelId} to note {noteId}");

                _context.SaveChanges();
            }
        }

        public void UpdateLabel(Label label)
        {
            _logger.LogInformation($"Updating label: {label}");

            _context.Labels.Update(label);
            _context.SaveChanges();
        }

        public void DeleteLabel(int labelId)
        {
            _logger.LogInformation($"Deleting label with ID: {labelId}");

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
*/


using DataAccessLayer.DataContext;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace DataAccessLayer.Repositories.Repo
{
    public class LabelRepository : ILabelRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<LabelRepository> _logger;
        private readonly IDatabase _redisDb;

        public LabelRepository(ApplicationDbContext context, ILogger<LabelRepository> logger, IConnectionMultiplexer redis)
        {
            _context = context;
            _logger = logger;
            _redisDb = redis.GetDatabase();
        }

        public IEnumerable<Label> GetLabelsByUserId(int userId)
        {
            string cacheKey = $"labels:user:{userId}";
            _logger.LogInformation($"Fetching labels for user ID: {userId}");

            // Check Redis cache
            var cachedLabels = _redisDb.StringGet(cacheKey);
            if (!cachedLabels.IsNullOrEmpty)
            {
                _logger.LogInformation("Cache hit for labels");
                return JsonSerializer.Deserialize<IEnumerable<Label>>(cachedLabels);
            }

            // Fetch from DB and cache the result
            var labels = _context.Labels
                .Where(label => label.NoteLabels.Any(nl => nl.note.UserId == userId))
                .Include(label => label.NoteLabels)
                .ToList();

            _redisDb.StringSet(cacheKey, JsonSerializer.Serialize(labels), TimeSpan.FromMinutes(10));
            return labels;
        }

        public IEnumerable<Label> GetLabelsByNoteId(int noteId)
        {
            string cacheKey = $"labels:note:{noteId}";
            _logger.LogInformation($"Fetching labels for note ID: {noteId}");

            // Check Redis cache
            var cachedLabels = _redisDb.StringGet(cacheKey);
            if (!cachedLabels.IsNullOrEmpty)
            {
                _logger.LogInformation("Cache hit for labels");
                return JsonSerializer.Deserialize<IEnumerable<Label>>(cachedLabels);
            }

            // Fetch from DB and cache the result
            var labels = _context.NoteLabels
                .Where(nl => nl.NoteId == noteId)
                .Select(nl => nl.label)
                .ToList();

            _redisDb.StringSet(cacheKey, JsonSerializer.Serialize(labels), TimeSpan.FromMinutes(10));
            return labels;
        }

        public void AddLabel(Label label)
        {
            _logger.LogInformation($"Adding label: {label}");
            _context.Labels.Add(label);
            _context.SaveChanges();

            // Clear cache after modification
            ClearAllLabelCaches();
        }

        public void AddLabelToNote(int noteId, int labelId)
        {
            if (!_context.NoteLabels.Any(nl => nl.NoteId == noteId && nl.LabelId == labelId))
            {
                _context.NoteLabels.Add(new NoteLabel { NoteId = noteId, LabelId = labelId });
                _logger.LogInformation($"Added label {labelId} to note {noteId}");

                _context.SaveChanges();

                // Clear cache after modification
                ClearLabelCacheByNoteId(noteId);
            }
        }

        public void UpdateLabel(Label label)
        {
            _logger.LogInformation($"Updating label: {label}");
            _context.Labels.Update(label);
            _context.SaveChanges();

            // Clear cache after update
            ClearAllLabelCaches();
        }

        public void DeleteLabel(int labelId)
        {
            _logger.LogInformation($"Deleting label with ID: {labelId}");

            var label = _context.Labels.FirstOrDefault(l => l.id == labelId);
            if (label != null)
            {
                _context.NoteLabels.RemoveRange(_context.NoteLabels.Where(nl => nl.LabelId == labelId));
                _context.Labels.Remove(label);
                _context.SaveChanges();

                // Clear cache after deletion
                ClearAllLabelCaches();
            }
        }

        private void ClearAllLabelCaches()
        {
            _redisDb.KeyDelete("labels:user:*");
            _redisDb.KeyDelete("labels:note:*");
        }

        private void ClearLabelCacheByNoteId(int noteId)
        {
            _redisDb.KeyDelete($"labels:note:{noteId}");
        }
    }
}
