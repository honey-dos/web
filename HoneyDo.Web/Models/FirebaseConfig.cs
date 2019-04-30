namespace HoneyDo.Web.Models
{
    /// <summary>
    /// Structure to hold firebase service accoung configuration.
    /// </summary>
    public class FirebaseConfig
    {
        /// <summary>
        /// Firebase api key.
        /// </summary>
        public string ApiKey { get; set; }
        /// <summary>
        /// Firebase auth domain.
        /// </summary>
        public string AuthDomain { get; set; }
        /// <summary>
        /// Firebase database url.
        /// </summary>
        public string DatabaseURL { get; set; }
        /// <summary>
        /// Firebase project id.
        /// </summary>
        public string ProjectId { get; set; }
    }
}