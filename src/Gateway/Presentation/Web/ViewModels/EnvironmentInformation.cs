
namespace Gateway.Web.Api.ViewModels
{
    /// <summary>
    /// provide information about application state to service.
    /// </summary>
    public class EnvironmentInformation
    {
        /// <summary>
        /// operating system
        /// </summary>
        public string OS { get; set; }
        /// <summary>
        /// computer network name
        /// </summary>
        public string MachineName { get; set; }
        /// <summary>
        /// runtime
        /// </summary>
        public string FrameworkVersion { get; set; }
        /// <summary>
        /// current hosting environment
        /// </summary>
        public string EnvironmentName { get; set; }
        /// <summary>
        /// latest commit hash
        /// </summary>
        public string CommitHash { get; set; }
        /// <summary>
        /// active branch
        /// </summary>
        public string Branch { get; set; }
        /// <summary>
        /// current tag name
        /// </summary>
        public string Tag { get; set; }
    }
}
