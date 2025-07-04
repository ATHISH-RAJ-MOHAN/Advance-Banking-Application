using Microsoft.IdentityModel.Tokens;

namespace AuthenticationAPI.Model
{
    public class AppSettings
    {
        public string? AppSecret { get; set; }
        public string? AppName { get; set; }
        public string? AppVersion { get; set; }

        private string _algorithm = SecurityAlgorithms.HmacSha256Signature;
        public string Algorithm
        {
            get { return _algorithm; }
            set
            {
                if (value.Contains("Hmac"))
                    _algorithm = SecurityAlgorithms.HmacSha256Signature;
                else if (value.Contains("Aes128Enc"))
                    _algorithm = SecurityAlgorithms.Aes128Encryption;
                else
                    _algorithm = value;
            }
        }
    }
}
