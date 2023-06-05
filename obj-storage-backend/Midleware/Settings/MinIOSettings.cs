namespace Midleware.Settings
{
    public class MinIOSettings
    {
        /// <summary>
        /// <Domain-name> or <ip:port> of your object storage
        /// </summary>
        public string EndPoint { get; set; }

        /// <summary>
        /// User ID that uniquely identifies your account
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Password to your account
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// boolean value to enable/disable HTTPS support
        /// </summary>
        public bool SupportHttps { get; set; }

    }
}
