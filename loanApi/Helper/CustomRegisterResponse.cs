namespace loanApi.Helper
{
    public class CustomRegisterResponse
    {
        public class CustomNewResponse
        {
            public string Message { get; set; }
            public bool Success { get; set; }
            public UserDetail UserDetails { get; set; }
        }

        public class UserDetail
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            // You may exclude Password from being returned in the response for security reasons.
            // public string Password { get; set; }
        }
    }
}
