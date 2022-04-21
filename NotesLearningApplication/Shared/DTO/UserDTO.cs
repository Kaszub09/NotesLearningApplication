using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotesLearningApplication.Shared.DTO
{
    public class UserDTO
    {
        [Required(AllowEmptyStrings = false)]
        public string Username { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        public string Password { get; set; }
    }
}
