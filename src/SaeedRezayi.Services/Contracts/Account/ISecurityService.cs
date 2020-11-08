using System;

namespace SaeedRezayi.Services.Contracts.Account
{
    public interface ISecurityService
    {
        string GetSha256Hash(string input);
        Guid CreateCryptographicallySecureGuid();
    }
}
