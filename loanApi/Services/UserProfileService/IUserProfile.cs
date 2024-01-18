using loanApi.Models;

namespace loanApi.Services.UserProfileService
{
    public interface IUserProfile
    {
        Task<UserProfile> GetUserProfile(int profileId);
        Task<bool> CreateProfile(UserProfile profile);
        Task<bool> UpdateProfile(int profileId, UserProfile request);
        Task<bool> Save();
    }
}
