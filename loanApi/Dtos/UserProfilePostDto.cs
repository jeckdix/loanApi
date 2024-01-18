using loanApi.Models;

namespace loanApi.Dtos
{
    public class UserProfilePostDto
    {
        public string BVN { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string PersonalAddress { get; set; }
        public string WorkAddress { get; set; }
        public string Occupation { get; set; }
        public decimal Salary { get; set; }
        public string EducationLevel { get; set; }
        public string NOKFullName { get; set; }
        public string NOKRelationship { get; set; }
        public string NOKOccupation { get; set; }
        public string NOKPhoneNumber { get; set; }
        public string NOKAddress { get; set; }
    }
}
