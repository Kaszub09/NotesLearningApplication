using NotesLearningApplication.Shared.DTO;

namespace DatabaseServices {
    public interface INotesDbService {
        Task<int> AddNoteAsync(NoteDTO noteDTO);
        IEnumerable<NoteDTO> GetAllNotes();
        IEnumerable<NoteDTO> GetAllNotes(int userId);
        IEnumerable<CategoryCountDTO> GetCountByCategory();
        NoteDTO GetNote(int noteId);
        Task ModifyNoteAsync(NoteDTO noteDTO);
        Task RemoveNoteAsync(int noteId);
    }
}