namespace HoneyDo.Infrastructure.Authentication
{
    public class LoginOptions
    {
        public string Issuer { get; set; }
        public long MillisecondsUntilExpiration { get; set; }
        public string Key { get; set; }
        public string FirebaseJson { get; set; }
    }
}
