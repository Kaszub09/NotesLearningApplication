using NotesLearningApplication.Shared.DTO;
using AutoMapper;
using System.Security.Cryptography;
using DatabaseServices.Entities;

using Microsoft.EntityFrameworkCore;

namespace DatabaseServices.Services {
    public class UsersDbService : ServiceBase, IUsersDbService {
        public UsersDbService(NotesDbContext dbContext, IMapper mapper) : base(dbContext, mapper) { }

        public User GetUser(UserInfoDTO userDTO) {
            return _dbContext.Users
                .Include(user => user.Claims)
                .First(user => user.Id == userDTO.Id);
        }
        public User GetUser(UserDTO userDTO) {
            return _dbContext.Users
                .Include(user => user.Claims)
                .First(user => user.Username.ToLower() == userDTO.Username.ToLower());
        }

        public async Task AddOrModifyClaimToUser(UserDTO userDTO, string claimKey, string claimValue) {
            var user = _dbContext.Users.Include(user => user.Claims).FirstOrDefault(user => user.Username.ToLower() == userDTO.Username.ToLower());
            if (user == null) {
                throw new KeyNotFoundException("User [" + userDTO.Username + "] not found.");
            } else {
                var claim = user.Claims.FirstOrDefault(c => c.Key.ToLower() == claimKey.ToLower());
                if (claim == null) {
                    user.Claims.Add(new DatabaseClaim() { UserId = user.Id, Key = claimKey, Value = claimValue });
                } else {
                    claim.Value = claimValue;
                }
                await _dbContext.SaveChangesAsync();
            }
        }
        public async Task<UserInfoDTO> AddUserAsync(UserDTO userDTO) {
            if (_dbContext.Users.Any(user => user.Username.ToLower() == userDTO.Username.ToLower())) {
                throw new ArgumentException("User [" + userDTO.Username + "] already exists in database.");
            } else {
                CreatePasswordHash(userDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);
                var newUser = new User() { Username = userDTO.Username,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt
                };

                await _dbContext.Users.AddAsync(newUser);
                await _dbContext.SaveChangesAsync();

                _dbContext.Claims.Add(new DatabaseClaim() { UserId = newUser.Id, Key = "UserId", Value = newUser.Id.ToString() });
                _dbContext.Claims.Add(new DatabaseClaim() { UserId = newUser.Id, Key = "Username", Value = newUser.Username });
                await _dbContext.SaveChangesAsync();

                return new UserInfoDTO() { Id = newUser.Id, Username = newUser.Username };
            }
        }

        public UserInfoDTO ValidateUserAsync(UserDTO userDTO) {
            var userToValidate = _dbContext.Users.FirstOrDefault(user => user.Username.ToLower() == userDTO.Username.ToLower());
            if (userToValidate == null) {
                throw new KeyNotFoundException("User [" + userDTO.Username + "] not found.");
            } else {
                if (!VerifyPasswordHash(userDTO.Password, userToValidate.PasswordSalt, userToValidate.PasswordHash)) {
                    throw new ArgumentException("Invalid password");
                }
                return new UserInfoDTO() { Id = userToValidate.Id, Username = userToValidate.Username };
            }
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt) {
            using (var hmac = new HMACSHA512()) {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordSalt, byte[] passwordHash) {
            using (var hmac = new HMACSHA512(passwordSalt)) {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

    }
}
