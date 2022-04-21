using Microsoft.EntityFrameworkCore;
using DatabaseServices.Entities;

namespace DatabaseServices {
    public class NotesDbContext: DbContext {
        public DbSet<Note> Notes { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<DatabaseClaim> Claims { get; set; }
        public DbSet<Category> Categories { get; set; }



        public NotesDbContext(DbContextOptions<NotesDbContext> options) : base(options) { }
    }
}
