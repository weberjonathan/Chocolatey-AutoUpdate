using System.Collections.Generic;

namespace CandyShop.Packages.Chocolatey
{
    public class ChocolateyPackage : IPackage
    {
        private readonly List<string> SUFFIXES = new List<string> {
            ".install",
            ".portable",
            ".app",
            ".commandline",
            ".tool"
        };

        public string Name { get; set; }
        
        public string Version { get; set; }
        
        public string AvailableVersion { get; set; }
        
        public bool Pinned { get; set; }

        /* Note that the installation of suffixed packages which have a meta package
         * does not require the installation of said meta package;
         * this means it is possible for a package to have a valid suffix but not have
         * a meta package in the scope of this application, if that meta package
         * was not installed on the system.
         * If both meta package and the suffixed package (like *.install) are installed
         * HasMetaPackage and HasSuffix should always match. This is the expected behavior
         * for most users. (Also note that the meta package has to be supplied manually
         * and is not determined by other properties, unlike HasSuffix.)
         */
        public ChocolateyPackage MetaPackage { get; set; }

        public bool HasMetaPackage => MetaPackage != null;
        
        
        public bool HasSuffix
        {
            get {
                int i = Name.LastIndexOf('.');
                return (i > 0) && SUFFIXES.Contains(Name.Substring(i));
            }
        }

        /// <summary>
        /// Removes valid suffixes from Name and returns it; Returns Name if no suffix is present.
        /// </summary>
        public string ClearName
        {
            get
            {
                int i = Name.LastIndexOf('.');
                return i > 0 ? Name.Substring(0, i) : Name;
            }
        }
    }
}
