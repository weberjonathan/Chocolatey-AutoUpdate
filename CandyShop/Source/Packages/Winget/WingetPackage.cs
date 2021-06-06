using System;

namespace CandyShop.Packages.Winget
{
    public class WingetPackage : IPackage
    {
        public string Name { get; set; }
        public String Id { get; set; }
        public string Version { get; set; }
        public string AvailableVersion { get; set; }
        public string Source { get; set; }
    }
}
