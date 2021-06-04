namespace CandyShop.Packages
{
    public interface IPackage
    {
        string Name { get; set; }
        string Version { get; set; }
        string AvailableVersion { get; set; }
    }
}
