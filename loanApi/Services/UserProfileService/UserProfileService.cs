using AutoMapper;
using loanApi.Data;
using loanApi.Helper;
using loanApi.Models;

namespace loanApi.Services.UserProfileService
{
    public class UserProfileService : IUserProfile
    {
        private readonly DataContext _context;

        public UserProfileService(DataContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateProfile(UserProfile profile)
        {
            await _context.UserProfiles.AddAsync(profile);
            return await Save();
        }

        public async Task<UserProfile> GetUserProfile(int profileId)
        {
            return await _context.UserProfiles.FindAsync(profileId);
            
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> UpdateProfile(int profileId, UserProfile request)
        {
            var profile = await _context.UserProfiles.FindAsync(profileId);

            if (profile == null)
                return false;


            profile.BVN = request.BVN;
            profile.PhoneNumber = request.PhoneNumber;
            profile.DOB = request.DOB;
            profile.Gender = request.Gender;
            profile.MaritalStatus = request.MaritalStatus;
            profile.PersonalAddress = request.PersonalAddress;
            profile.WorkAddress = request.WorkAddress;
            profile.Occupation = request.Occupation;
            profile.Salary = request.Salary;
            profile.EducationLevel = request.EducationLevel;
            profile.NOKFullName = request.NOKFullName;
            profile.NOKRelationship = request.NOKRelationship;
            profile.NOKOccupation = request.NOKOccupation;
            profile.NOKPhoneNumber = request.NOKPhoneNumber;
            profile.NOKAddress = request.NOKAddress;

            var bvnCheck = await ProfileUpdateCheckers.BVNCheckAsync(request.BVN);
            var phoneCheck = await ProfileUpdateCheckers.PhoneNumberCheckAsync(request.PhoneNumber);
            var salaryCheck = await ProfileUpdateCheckers.SalaryCheck(request.Salary);

            if ( bvnCheck && phoneCheck && salaryCheck)
            {
                profile.ProfileUpdated = true;
            }

            return await Save();
        }


    }
}
