using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CandyShop.Packages.Winget
{
    public class WingetManager : IPackageManager
    {
        // TODO implement caches here, because GetOutdated also calls GetInstalled

        public string GetInfo(IPackage package)
        {
            return "";
        }

        public async Task<string> GetInfoAsync(IPackage package)
        {
            return "";
        }

        public List<IPackage> GetInstalled()
        {
            // TODO make sure dequeues do not throw bc of missing elements
            
            // launch process
            WingetProcess p = new WingetProcess($"list");
            p.ExecuteHidden();

            // remove download indicators from output by skipping to first mention of "Name"
            int i = 0;
            while (p.Output[i] != 'N')
            {
                i++;
            }
            
            Queue<string> output = new Queue<string>(p.Output[i..].Split(Environment.NewLine));

            string header = output.Dequeue();
            if (!header.StartsWith("Name"))
            {
                throw new PackageManagerException(); // TODO invalid
            }

            int nameIndex = 0;
            int idIndex = GetNextColumnIndex(header, nameIndex);
            int versionIndex = GetNextColumnIndex(header, idIndex);
            int availableIndex = GetNextColumnIndex(header, versionIndex);
            int sourceIndex = GetNextColumnIndex(header, availableIndex);

             if (idIndex == 0 || versionIndex == 0 || availableIndex == 0 || sourceIndex == 0)
            {
                throw new PackageManagerException(); // TODO why tf does this happen sometimes? 
            }

            string divider = output.Dequeue();
            if (!divider.StartsWith('-') && sourceIndex < divider.Length)
            {
                throw new PackageManagerException(); // TODO
            }

            List<WingetPackage> rtn = new List<WingetPackage>();
            while (output.Count > 0)
            {
                string row = output.Dequeue();
                if (String.IsNullOrEmpty(row))
                {
                    continue;
                }

                WingetPackage package = new WingetPackage();
                package.Name = row.Substring(nameIndex, idIndex).Trim();
                package.Id = row.Substring(idIndex, versionIndex - idIndex).Trim();
                package.Version = row.Substring(versionIndex, availableIndex - versionIndex).Trim();
                package.AvailableVersion = row[availableIndex..sourceIndex].Trim();
                package.Source = row.Substring(sourceIndex).Trim();

                rtn.Add(package);
            }

            return new List<IPackage>(rtn);
        }

        public async Task<List<IPackage>> GetInstalledAsync()
        {
            return await Task.Run(GetInstalled);
        }

        public List<IPackage> GetOutdated()
        {
            List<IPackage> installedPackages = GetInstalled();
            List<IPackage> rtn = new List<IPackage>();

            installedPackages.ForEach((IPackage p) => {
                if (!String.IsNullOrEmpty(p.AvailableVersion))
                {
                    rtn.Add(p);
                }
            });

            return rtn;
        }

        public async Task<List<IPackage>> GetOutdatedAsync()
        {
            return await Task.Run(GetOutdated);
        }

        public void Upgrade(List<IPackage> packages)
        {
            string packageNames = "";
            foreach (WingetPackage pckg in packages) // TODO make cast safe?
            {
                packageNames += pckg.Id + " ";
            }

            // launch process
            WingetProcess p = new WingetProcess($"upgrade {packageNames}");
            p.Execute();
        }

        private int GetNextColumnIndex(string row, int startIndex)
        {
            int i = startIndex;
            while (i < row.Length && Char.IsLetterOrDigit(row[i]))
            {
                i++;
            }

            while (i < row.Length && Char.IsWhiteSpace(row[i]))
            {
                i++;
            }

            return i;
        }
    }
}
