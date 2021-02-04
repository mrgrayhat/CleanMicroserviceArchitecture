using System.Runtime.InteropServices;
using BlogModule.Application.Abstractions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace BlogModule.Infrastructure.Environment
{
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
