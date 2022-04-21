using AutoMapper;
using DatabaseServices.Entities;
using Microsoft.EntityFrameworkCore;
using NotesLearningApplication.Shared.DTO;

namespace DatabaseServices {
    public class NotesDbService : INotesDbService {
        NotesDbContext _dbContext;
        IMapper _mapper;

        public NotesDbService(NotesDbContext dbContext, IMapper mapper) {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<int> AddNoteAsync(NoteDTO noteDTO) {
            var newNote = _mapper.Map<Note>(noteDTO);
            newNote.CreatedAt = DateTime.Now;

            if (_dbContext.Notes.Any(note => note.Title.ToLower() == newNote.Title.ToLower() && note.UserId == newNote.UserId)) {
                throw new ArgumentException($"Note [{newNote.Title}] for for user {newNote.UserId} already exists in database.");
            } else {
                await _dbContext.Notes.AddAsync(newNote);
                await _dbContext.SaveChangesAsync();
            }
            return newNote.Id;
        }

        public async Task RemoveNoteAsync(int noteId) {
            var noteToRemove = _dbContext.Notes.FirstOrDefault(note => note.Id == noteId);
            if (noteToRemove == null) {
                throw new KeyNotFoundException($"Id [{noteId}] not found in database.");
            } else {
                _dbContext.Notes.Remove(noteToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task ModifyNoteAsync(NoteDTO noteDTO) {
            var noteToModify = _dbContext.Notes.FirstOrDefault(note => note.Id == noteDTO.Id);
            if (noteToModify == null) {
                throw new KeyNotFoundException($"Id [{noteDTO.Id}] not found in database.");
            } else {
                noteToModify.Title = noteDTO.Title;
                noteToModify.Description = noteDTO.Description;
                noteToModify.CategoryId = noteDTO.CategoryId;
                await _dbContext.SaveChangesAsync();
            }
        }

        public NoteDTO GetNote(int noteId) {
            var noteToGet = _dbContext.Notes.Include(n => n.Category).FirstOrDefault(note => note.Id == noteId);
            if (noteToGet == null) {
                throw new KeyNotFoundException($"Id [{noteId}] not found in database.");
            } else {
                return _mapper.Map<NoteDTO>(noteToGet);
            }
        }

        public IEnumerable<NoteDTO> GetAllNotes(int userId) {
            return _mapper.Map<List<NoteDTO>>(_dbContext.Notes.Include(n => n.Category).Where(note => note.UserId == userId).ToList());
        }
        public IEnumerable<NoteDTO> GetAllNotes() {
            return _mapper.Map<List<NoteDTO>>(_dbContext.Notes.ToList());
        }

        public IEnumerable<CategoryCountDTO> GetCountByCategory() {
            return _dbContext
                .Notes
                .Include(n => n.Category)
                .GroupBy(n => n.CategoryId)
                .Select(item => new CategoryCountDTO() { Id = item.Key, Count = item.Count(), Name = item.First().Category.Name });
        }
    }
}

