using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DatabaseServices.Entities {
    public class Note {
        [Key]
        public int Id { get; set; }
        [Required(AllowEmptyStrings =false)]
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
    }
}
