namespace TokenUtility.Configuration
{
    public class JwtSecrets:IJwtSecrets
    {
        public string userTokenKey { get; set; }
        public string passwordResetKey { get; set; }
    }
    public interface IJwtSecrets
    {
        public string userTokenKey { get; set; }
        public string passwordResetKey { get; set; }
    }
}
