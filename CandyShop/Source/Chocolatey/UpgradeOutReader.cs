using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CandyShop.Chocolatey
{
    public class UpgradeOutReader
    {
        private enum States
        {
            Begin,
            Header,
            PackageList,
            Disclaimer,
            PackageHeader,
            PackageProgress,
            PackageInstall,
            Summary,
            End
        }
        
        private readonly ChocolateyUpgradeMonitor _Monitor;

        /* STATES
         * represent expected input in ReadNext(string)
         * 0  (START) version check                 -> 1
         * 1  "Upgrading the following packages:"   -> 2
         * 3  packages to upgrade                   -> 4
         * 4  package downloading header            -> 4 | 5
         * 5  package download progress             -> 5 | 6
         * 6  package install script                -> 6 | 7
         * 7  Chocolatey summary                    -> 8
         * 8  list of upgraded packages             -> END
         * 
         */

        private States state = States.Begin;
        private int totalPckgCount = 0;
        private int currentPckgIndex = 0;

        private string currentPckgName = "";
        private string currentPckgVersion = "";
        private string currentPckgNewVersion = "";

        public UpgradeOutReader(ChocolateyUpgradeMonitor monitor)
        {
            _Monitor = monitor;
        }

        /// <exception cref="ChocolateyParsingException"></exception>
        public void ReadNext(string line)
        {
            if (States.Begin.Equals(state))
            {
                // TODO version
                state = States.Header;
            }
            else if (States.Header.Equals(state))
            {
                // Upgrading the following packages:
                state = States.PackageList;
            }
            else if (States.PackageList.Equals(state))
            {
                if (line.Equals("all"))
                {
                    // also see state 4
                    throw new ChocolateyParsingException("Unexpected upgrading of packages 'all'");
                }

                string[] packages = line.Split(';');
                // TODO create queue of individual packages?
                
                totalPckgCount = packages.Length;
                state = States.Disclaimer;
            }
            else if (States.Disclaimer.Equals(state))
            {
                // validate "By upgrading you accept licenses for the packages."
                // contine on empty line
                if ("".Equals(line))
                {
                    state = States.PackageHeader;
                }
            }
            else if (States.PackageHeader.Equals(state))
            {
                /* Can be the start of section for upgrading package
                 * - "You have autohotkey.portable v1.1.33.02 installed. Version 1.1.33.03 is available based on your source(s)."
                 * - "7zip v19.0 is the latest version available based on your source(s)."
                 *   this case can be ignored, since it is not expected to occurr when excluding "choco upgrade all"
                 *   which is done in state 2
                 */


                string pattern = "^(You have \\S* \\S* installed. Version \\S* is available based on your source\\(s\\)\\.)";
                
                if (Regex.IsMatch(line, pattern))
                {
                    string[] words = line.Split(' ');

                    currentPckgName = words[2];
                    currentPckgVersion = words[3];
                    currentPckgNewVersion = words[6];

                    currentPckgIndex++;
                }
                else if ("".Equals(line))
                {
                    state = States.PackageProgress;
                }
                else
                {
                    throw new ChocolateyParsingException("Could not find expected pattern in state 4."); // TODO phrase properly
                }
            }
            else if (States.PackageProgress.Equals(state))
            {
                int progress;

                // ends with empty line; so change state based on that

                if ("".Equals(line))
                {
                    state = States.PackageInstall;
                }
                else
                {
                    Match match = Regex.Match(line, $"^Progress: Downloading {currentPckgName} {currentPckgNewVersion}... (\\d+)%$");
                    if (match.Success)
                    {
                        int.TryParse(match.Groups[1].Value, out progress); // TODO test wtf match.Value is now; hopefully: progress number; else check regex groups
                    }
                    else
                    {
                        throw new ChocolateyParsingException($"Expected progress report for package {currentPckgName}");
                    }

                    //_Monitor.PackageProgress = progress; // TODO TEST
                }
            }
            else if (States.PackageInstall.Equals(state))
            {
                // this is where install starts; TODO implement stuff
                // for now: skip until empty line, marking end of install
                // TODO implementation necessary to figure out errors and be able to report them, if install failed (for example bc of non matching keys)

                if ("".Equals(line))
                {
                    // _Monitor.FinishedPackageCount++; // TODO TEST

                    if (totalPckgCount == currentPckgIndex + 1)
                    {
                        // all packages finished
                        state = States.Summary;
                    }
                    else
                    {
                        // download next
                        state = States.PackageHeader;
                    }
                }
            }
            else if (States.Summary.Equals(state))
            {
                // TODO the "upgraded:" section is not there if only 1 package is upgraded
                
                if (Regex.IsMatch(line, "^(Chocolatey upgraded \\d/\\d packages\\.)$"))
                {
                    // TODO noop
                }
                else if (Regex.IsMatch(line, "^\\sSee the log for details \\(.+\\)\\.$"))
                {
                    // TODO noop
                    state = States.End;
                }
                else if ("".Equals(line))
                {
                    // TODO noop
                }
                else if ("Upgraded:".Equals(line))
                {
                    state = States.End;
                }
            }
            else if (States.End.Equals(state))
            {
                
            }
        }

    }
}
