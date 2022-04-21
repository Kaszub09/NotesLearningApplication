using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DatabaseServices.Entities;

namespace DatabaseServices {
    public class NotesDbSeeder {
        private NotesDbContext _context;

        public NotesDbSeeder(NotesDbContext context) {
            _context = context;
        }

        public void Seed() {
            if (_context.Database.CanConnect()) {
                if (!_context.Categories.Any()) {
                    _context.Categories.AddRange(GetCategories());
                    _context.SaveChanges();
                }

                if (!_context.Users.Any())
                {
                    _context.Users.AddRange(GetUsers());
                    _context.SaveChanges();
                }
                if (!_context.Claims.Any()) {
                    _context.Claims.AddRange(GetClaims());
                    _context.SaveChanges();
                }
                if (!_context.Notes.Any())
                {
                    _context.Notes.AddRange(GetNotes());
                    _context.SaveChanges();
                }
            }
        }

        private IEnumerable<Category> GetCategories() {
            return new List<Category>() {
                new Category(){Name = "Important",Description="Very important category"},
                new Category(){Name = "Less important",Description="Less important category"},
                new Category(){Name = "Reminder",Description="Category for reminders"},
                new Category(){Name = "TO DO",Description="Category for items to do"},
                new Category(){Name = "Warning",Description="Category for warnings"},
                new Category(){Name = "Sample",Description="Some sample category"}
            };
        }

        private IEnumerable<DatabaseClaim> GetClaims() {
            return new List<DatabaseClaim>() {
                new DatabaseClaim(){UserId=1,Key=ClaimTypes.Role,Value="Admin"},
                 new DatabaseClaim(){UserId=1,Key="UserId",Value="1"},
                  new DatabaseClaim(){UserId=1,Key="Username",Value="Admin"}
            };
        }

        private IEnumerable<User> GetUsers() {
            using (var hmac = new HMACSHA512()) {
                return new List<User>() {
                    new User(){Username="admin",
                        PasswordHash=hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("admin")),
                        PasswordSalt = hmac.Key
                    }
                };
            }
        }

        private IEnumerable<Note> GetNotes() {
            return new List<Note>() {
                new Note(){UserId=1,Description="This is note nr 1",Title="Note 1",CreatedAt = DateTime.Now},
                new Note(){CategoryId=_context.Categories.First().Id,UserId=1,Description="This 2nd note",Title="Note 2",CreatedAt = DateTime.Now}
            };
        }

    }
}

