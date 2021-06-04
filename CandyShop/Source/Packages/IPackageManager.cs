using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandyShop.Packages
{
    interface IPackageManager
    {
        void Upgrade(List<IPackage> packages);
        List<IPackage> GetOutdated();
        List<IPackage> GetInstalled();
        string GetInfo(IPackage package);

        Task<List<IPackage>> GetOutdatedAsync();
        Task<List<IPackage>> GetInstalledAsync();
        Task<string> GetInfoAsync(IPackage package);
    }
}
