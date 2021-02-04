using System.Security.Cryptography.X509Certificates;

namespace STS.Infrastructure.Services.Certificate
{
    public interface ICertificateService
    {
        X509Certificate2 GetCertificateFromKeyVault(string vaultCertificateName);
    }
}