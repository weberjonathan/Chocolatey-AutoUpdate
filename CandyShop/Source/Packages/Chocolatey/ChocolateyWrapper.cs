using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CandyShop.Packages.Chocolatey
{
    public class ChocolateyWrapper : IPackageManager
    {
        /* Parsing Chocolatey data
         *  Choco may add disclaimers and other text either above
         *  or below the desired output, which needs to be filtered
         *  out. These texts are usually seperated from desired out
         *  by empty lines, so the entire output can be organized in
         *  blocks seperated by empty lines. Unwanted blocks can be 
         *  identified and discarded.
         *  
         *  ChocolateyProcess.FormattedOutput provides these blocks
         *  for hidden ChocolateyProcesses.
         *  
         *  Note that the desired output may contain empty lines
         *  aswell, which means it may stretch over multiple
         *  consecutive blocks. See individual functions for clarity.
         *  
         *  The output always starts with a version string like
         *  "Chocolatey v0.10.15".
         */

        /// <exception cref="ChocolateyException"></exception>
        public void Upgrade(List<IPackage> packages)
        {
            string argument = "";
            foreach (ChocolateyPackage pckg in packages)
            {
                argument += pckg.Name + " ";
            }

            // launch process
            ChocolateyProcess p = new ChocolateyProcess($"upgrade {argument} -y");
            p.Execute();
        }

        /// <exception cref="ChocolateyException"></exception>
        public List<IPackage> GetOutdated()
        {
            List<ChocolateyPackage> packages = new List<ChocolateyPackage>();

            // launch process
            ChocolateyProcess p = new ChocolateyProcess("outdated");
            p.ExecuteHidden();

            // parse (find header block, followed by package block, followed by summary block)
            List<List<string>> blocks = p.FormattedOutput;
            for (int i = 0; i < blocks.Count; i++)
            {
                // find header block
                if (blocks[i].Count == 2 &&
                    blocks[i][0].Equals("Outdated Packages") &&
                    blocks[i][1].Equals(" Output is package name | current version | available version | pinned?") &&
                    i + 2 < blocks.Count)
                {
                    List<string> packageList = blocks[i + 1];
                    List<string> summary = blocks[i + 2];

                    // check summary
                    int outdatedCount;
                    if (summary.Count == 1 &&
                        summary[0].StartsWith("Chocolatey has determined ") &&
                        summary[0].EndsWith(" package(s) are outdated. ") &&
                        summary[0].Length > 51 &&
                        int.TryParse(summary[0].Substring(26, summary[0].Length - 51), out outdatedCount))
                    {
                        if (outdatedCount == packageList.Count)
                        {
                            // check packages
                            foreach (string line in packageList)
                            {
                                // retrieve package
                                string[] entry = line.Split('|');
                                if (entry.Length == 4)
                                {
                                    ChocolateyPackage pckg = new ChocolateyPackage();
                                    pckg.Name = entry[0];
                                    pckg.Version = entry[1];
                                    pckg.AvailableVersion = entry[2];
                                    pckg.Pinned = entry[3].Equals("true");
                                    packages.Add(pckg);
                                }
                            }
                        }
                    }
                }
            }

            return new List<IPackage>(addMetaPackageInformation(packages));
        }

        /// <exception cref="ChocolateyException"></exception>
        public async Task<List<IPackage>> GetOutdatedAsync()
        {
            return await Task.Run(GetOutdated);
        }

        /// <exception cref="ChocolateyException"></exception>
        public List<IPackage> GetInstalled()
        {
            List<ChocolateyPackage> packages = new List<ChocolateyPackage>();

            // launch process TODO try catch
            ChocolateyProcess p = new ChocolateyProcess("list --local-only");
            p.ExecuteHidden();

            // parse output (get blocks, all wanted info is in single block)
            // conditions
            // - final line looks like "54 packages included."; match count
            // - package list entries look like "name version"
            List<List<string>> blocks = p.FormattedOutput;
            foreach (List<string> block in blocks)
            {
                // check final line and match count
                string summary = block[block.Count - 1];
                if (summary.Contains(' '))
                {
                    int indexOf = summary.IndexOf(' ');
                    string packageCountStr = summary.Substring(0, indexOf);
                    int packageCount;

                    if (int.TryParse(packageCountStr, out packageCount))
                    {
                        if (packageCount == block.Count - 1 && summary.Substring(indexOf).Equals(" packages installed."))
                        {
                            // check potential package list entries
                            for (int i = 0; i < block.Count - 1; i++)
                            {
                                if (block[i].Contains(' '))
                                {
                                    // retrieve package
                                    string[] entry = block[i].Split(' ');
                                    if (entry.Length == 2)
                                    {
                                        ChocolateyPackage pckg = new ChocolateyPackage();
                                        pckg.Name = entry[0];
                                        pckg.Version = entry[1];
                                        packages.Add(pckg);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return new List<IPackage>(addMetaPackageInformation(packages));
        }

        /// <exception cref="ChocolateyException"></exception>
        public async Task<List<IPackage>> GetInstalledAsync()
        {
            return await Task.Run(GetInstalled);
        }

        /// <exception cref="ChocolateyException"></exception>
        public string GetInfo(IPackage package)
        {
            StringBuilder rtn = new StringBuilder();

            // launch process
            ChocolateyProcess p = new ChocolateyProcess($"info {package.Name}");
            p.ExecuteHidden();

            // parse
            List<List<string>> blocks = p.FormattedOutput;
            foreach (List<string> block in blocks)
            {
                if (block.Count > 0)
                {
                    for (int i = 0; i < block.Count; i++)
                    {
                        rtn.AppendLine(block[i].Trim());
                    }
                }
            }

            return rtn.ToString();
        }

        /// <exception cref="ChocolateyException"></exception>
        public async Task<string> GetInfoAsync(IPackage package)
        {
            return await Task.Run(() => GetInfo(package));
        }

        private List<ChocolateyPackage> addMetaPackageInformation(List<ChocolateyPackage> packages)
        {
            foreach (ChocolateyPackage childPackageCandidate in packages)
            {
                if (childPackageCandidate.HasSuffix)
                {
                    foreach (ChocolateyPackage metaPackageCandidate in packages)
                    {
                        if (childPackageCandidate.ClearName.Equals(metaPackageCandidate.Name))
                        {
                            childPackageCandidate.MetaPackage = metaPackageCandidate;
                        }
                    }
                }
            }

            return packages;
        }
    }
}
