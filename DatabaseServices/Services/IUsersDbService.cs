using DatabaseServices.Entities;
using NotesLearningApplication.Shared.DTO;

namespace DatabaseServices.Services {
    public interface IUsersDbService {
        Task AddOrModifyClaimToUser(UserDTO userDTO, string claimKey, string claimValue);
        Task<UserInfoDTO> AddUserAsync(UserDTO userDTO);
        User GetUser(UserDTO userDTO);
        User GetUser(UserInfoDTO userDTO);
        UserInfoDTO ValidateUserAsync(UserDTO userDTO);
    }
}