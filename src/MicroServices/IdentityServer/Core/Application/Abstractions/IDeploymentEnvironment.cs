namespace STS.Application.Abstractions
{
    public interface IDeploymentEnvironment
    {
        string OS { get; }
        string MachineName { get; }
        string RuntimeFramework { get; }
        string EnvironmentName { get; }
        string CommitSha { get; }
        string Branch { get; }
        string Tag { get; }
    }
}
