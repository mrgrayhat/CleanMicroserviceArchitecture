using System.Runtime.InteropServices;
using Gateway.Web.Api.Services.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Gateway.Web.Api.Services
{
    /// <summary>
    /// Development and Deploy State Information Service.
    /// </summary>
    public class DeploymentEnvironment : IDeploymentEnvironment
    {
        private readonly ILogger<DeploymentEnvironment> _logger;
        private readonly IWebHostEnvironment _hostingEnv;

        public DeploymentEnvironment(IWebHostEnvironment hostingEnv, ILogger<DeploymentEnvironment> logger)
        {
            _hostingEnv = hostingEnv;
            _logger = logger;
        }

        public string OS => $"{RuntimeInformation.OSDescription} {RuntimeInformation.OSArchitecture}";
        public string MachineName => System.Environment.MachineName;
        public string RuntimeFramework =>
            $"{RuntimeInformation.FrameworkDescription} {RuntimeInformation.ProcessArchitecture}";
        public string EnvironmentName => _hostingEnv.EnvironmentName;

        public string CommitSha => ThisAssembly.Git.Commit;

        public string Branch => ThisAssembly.Git.Branch;
        public string Tag => ThisAssembly.Git.Tag;
    }
}
