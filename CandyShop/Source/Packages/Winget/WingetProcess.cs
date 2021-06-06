using System.ComponentModel;
using System.Diagnostics;
using System.Text;

namespace CandyShop.Packages.Winget
{
    public class WingetProcess
    {
        private const string BIN = "winget";

        public string Output { get; private set; } = "";

        public string Args { get; private set; } = "";

        public WingetProcess(string args)
        {
            Args = args;
        }

        /// <summary>
        ///     Executes the winget process without creating a window
        ///     and writes stdout to the Output property after execution
        /// </summary>
        /// <exception cref="PackageManagerException"></exception>
        public void ExecuteHidden()
        {
            ProcessStartInfo procInfo = new ProcessStartInfo(BIN, Args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
                StandardOutputEncoding = Encoding.Default
            };

            try
            {
                Process proc = Process.Start(procInfo);
                string output = proc.StandardOutput.ReadToEnd();

                proc.WaitForExit();
                var test = proc.StandardOutput.ReadToEnd();
                Output = output;

                if (proc.ExitCode != 0)
                {
                    // TODO what's in output => add property for stderr? put stderr in output?
                    throw new PackageManagerException($"winget did not exit cleanly. Returned {proc.ExitCode}.");
                }
            }
            catch (Win32Exception e)
            {
                // TODO what's in output => add property for stderr? put stderr in output?
                throw new PackageManagerException("An error occurred while running choco.", e);
            }
        }

        /// <summary>
        ///     Executes the winget process in a new console
        /// </summary>
        /// <exception cref="PackageManagerException"></exception>
        public void Execute()
        {
            // TODO potentially redirect output and expose events

            ProcessStartInfo procInfo = new ProcessStartInfo(BIN, Args)
            {
                UseShellExecute = false,
            };

            try
            {
                Process proc = Process.Start(procInfo);
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new PackageManagerException($"winget did not exit cleanly. Returned {proc.ExitCode}.");
                }
            }
            catch (Win32Exception e)
            {
                throw new PackageManagerException("An error occurred while running choco.", e);
            }
        }
    }
}
