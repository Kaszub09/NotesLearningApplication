using DatabaseServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NotesLearningApplication.Shared.DTO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace NotesLearningApplication.Server.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase {
        INotesDbService _notesDbService;
        public NotesController(INotesDbService notesDbService) {
            _notesDbService = notesDbService;
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
        public ActionResult<IEnumerable<NoteDTO>> GetAllNotes() {
            return Ok(_notesDbService.GetAllNotes());
        }

        [HttpGet("count")]
        public ActionResult<IEnumerable<int>> GetCount() {
            return Ok(_notesDbService.GetAllNotes().Count());
        }
        [HttpGet("count/categories")]
        public ActionResult<IEnumerable<CategoryCountDTO>> GetCountByCategory() {
            return Ok(_notesDbService.GetCountByCategory());
        }


        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<NoteDTO> GetNote(int id) {
            if (User.Claims.First(c => c.Type == "UserId").Value == id.ToString()) {
                return Ok(_notesDbService.GetNote(id));
            } else {
                return Unauthorized();
            }    
        }

        [HttpGet("user/{id}")]
        [Authorize]
        public ActionResult<IEnumerable<NoteDTO>> GetUserNotes(int id) {
            if (User.Claims.First(c => c.Type == "UserId").Value == id.ToString()) {
                return Ok(_notesDbService.GetAllNotes(id));
            } else {
                return Unauthorized();
            }
        }

        [HttpPost("new")]
        [Authorize]
        public async Task<ActionResult<int>> Post([FromBody] NoteDTO noteDTO) {
            noteDTO.UserId = int.Parse(User.Claims.First(c => c.Type == "UserId").Value);
            return Ok(await _notesDbService.AddNoteAsync(noteDTO));
        }

        [HttpPut("edit")]
        [Authorize]
        public async Task<ActionResult> Put([FromBody] NoteDTO noteDTO) {
            if (User.Claims.First(c => c.Type == "UserId").Value == noteDTO.UserId.ToString()) {
                await _notesDbService.ModifyNoteAsync(noteDTO);
                return Ok();
            } else {
                return Unauthorized();
            }
        }
 
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult> Delete(int id) {
            if (User.Claims.First(c => c.Type == "UserId").Value == _notesDbService.GetNote(id).UserId.ToString()) {
                await _notesDbService.RemoveNoteAsync(id);
                return Ok();
            } else {
                return Unauthorized();
            }
        }
    }
}
