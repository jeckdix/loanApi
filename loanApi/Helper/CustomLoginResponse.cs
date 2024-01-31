namespace loanApi.Helper
{
    public class CustomLoginResponse
    {
        public class LoginResponse
        {
            public string Token { get; set; }
            public Detail UserDetails { get; set; }
        }

        public class Detail
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            // You may exclude Password from being returned in the response for security reasons.
            // public string Password { get; set; }
        }
    }
}
