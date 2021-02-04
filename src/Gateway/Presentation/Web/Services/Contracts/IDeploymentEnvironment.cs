namespace Gateway.Web.Api.Services.Contracts
{
    public interface IDeploymentEnvironment
    {
        /// <summary>
        /// operating system
        /// </summary>
        string OS { get; }
        /// <summary>
        /// computer network name
        /// </summary>
        string MachineName { get; }
        /// <summary>
        /// runtime
        /// </summary>
        string RuntimeFramework { get; }
        /// <summary>
        /// current hosting environment
        /// </summary>
        string EnvironmentName { get; }
        /// <summary>
        /// latest commit hash
        /// </summary>
        string CommitSha { get; }
        /// <summary>
        /// active branch
        /// </summary>
        string Branch { get; }
        /// <summary>
        /// current tag name
        /// </summary>
        string Tag { get; }
    }
}
