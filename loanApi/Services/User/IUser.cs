namespace loanApi.Services.User
{
    public interface IUser
    {
        public Task<int?> GetUserByEmail(string email);
    }
}
