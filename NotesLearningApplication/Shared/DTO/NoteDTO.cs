using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLearningApplication.Shared.DTO {
    public class NoteDTO {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        [Required]
        public int? CategoryId { get; set; }
        public virtual CategoryDTO? Category { get; set; }
        public int? UserId { get; set; }

        public NoteDTO ShallowCopy() {
            return (NoteDTO)this.MemberwiseClone();
        }
    }
}
