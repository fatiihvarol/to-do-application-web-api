namespace to_do_application_web_api.Data.Schema.Auth
{
    public class AuthResponseVM
    {
        public string Token { get; set; }
        public string RefreshToken { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
