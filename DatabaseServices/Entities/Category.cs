using System.ComponentModel.DataAnnotations;

namespace DatabaseServices.Entities {
    public class Category {
        public int Id { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}
