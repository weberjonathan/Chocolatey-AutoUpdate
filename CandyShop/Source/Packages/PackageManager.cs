using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandyShop.Packages
{
    public class PackageManager : IPackageManager
    {
        public string GetInfo(IPackage package)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetInfoAsync(IPackage package)
        {
            throw new NotImplementedException();
        }

        public List<IPackage> GetInstalled()
        {
            throw new NotImplementedException();
        }

        public Task<List<IPackage>> GetInstalledAsync()
        {
            throw new NotImplementedException();
        }

        public List<IPackage> GetOutdated()
        {
            throw new NotImplementedException();
        }

        public Task<List<IPackage>> GetOutdatedAsync()
        {
            throw new NotImplementedException();
        }

        public void Upgrade(List<IPackage> packages)
        {
            throw new NotImplementedException();
        }
    }
}
