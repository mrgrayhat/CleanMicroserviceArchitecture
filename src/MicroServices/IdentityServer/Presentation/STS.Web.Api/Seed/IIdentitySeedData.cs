using System;

namespace STS.Web.Seed
{
    public interface IIdentitySeedData
    {
        void Seed(IServiceProvider serviceProvider);
    }
}