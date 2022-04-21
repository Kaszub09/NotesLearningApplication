using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLearningApplication.Shared.DTO {
    public class CategoryDTO {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
