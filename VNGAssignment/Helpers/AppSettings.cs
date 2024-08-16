namespace VNGAssignment.Helpers
{
    public class ConnectionStrings
    {
        public string ConnectionString { get; set; }
        public string DataProvider { get; set; }
    }

    public class JwtSettings
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }
        public string ExpiryMinutes { get; set; }
    }
}
