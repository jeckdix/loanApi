namespace loanApi.Helper
{
    public class ProfileUpdateCheckers
    {
        public static async Task<bool> BVNCheckAsync(string bvn)
        {
            bool isIntValues = Int64.TryParse(bvn, out _);
            bool isElevenDigits = bvn.Length == 11;

            return await Task.FromResult(isIntValues && isElevenDigits);
        }

        public static async Task<bool> PhoneNumberCheckAsync(string phone)
        {
            bool isIntValues = Int64.TryParse(phone, out _);
            bool isElevenDigits = phone.Length == 11;

            return await Task.FromResult(isIntValues && isElevenDigits);
        }

        public static async Task<bool> SalaryCheck(decimal salary)
        {
            var salaryUpdated = salary != 0.0M;
            return await Task.FromResult(salaryUpdated);
        }
    }
}
