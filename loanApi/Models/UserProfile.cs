namespace loanApi.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string BVN { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public DateTime DOB { get; set; } 
        public Gender Gender { get; set; }
        public MaritalStatus MaritalStatus { get; set; }
        public string PersonalAddress { get; set; } = string.Empty;
        public string WorkAddress { get; set; } = string.Empty;
        public string Occupation { get; set; } = string.Empty;
        public decimal Salary { get; set; }
        public string EducationLevel { get; set; } = string.Empty;
        public string NOKFullName { get; set; } = string.Empty;
        public string NOKRelationship { get; set; } = string.Empty;
        public string NOKOccupation { get; set; } = string.Empty;
        public string NOKPhoneNumber { get; set; } = string.Empty;
        public string NOKAddress { get; set; } = string.Empty;
        public bool ProfileUpdated { get; set; } = false;

        public int UserId { get; set; }
        public User User { get; set; }

    }

    public enum Gender
    {
        m,
        f,
        other
    }

    public enum MaritalStatus
    {
        single,
        married
    }
}
