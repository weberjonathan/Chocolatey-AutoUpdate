using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

namespace CandyShop.Packages.Chocolatey
{
    public class ChocolateyProcess
    {
        private const string CHOCO_BIN = "choco";

        private string Args;

        public ChocolateyProcess(string args)
        {
            Args = args;
        }

        public string Output { get; private set; } = "";

        public List<List<string>> FormattedOutput { get; private set; } = new List<List<string>>();

        /// <exception cref="ChocolateyException"></exception>
        public void ExecuteHidden()
        {
            ProcessStartInfo procInfo = new ProcessStartInfo(CHOCO_BIN, Args)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            try
            {
                Process proc = Process.Start(procInfo);
                string output = proc.StandardOutput.ReadToEnd();

                proc.WaitForExit();
                Output = output;

                if (proc.ExitCode != 0)
                {
                    throw new ChocolateyException($"choco did not exit cleanly. Returned {proc.ExitCode}.");
                }
            }
            catch (Win32Exception e)
            {
                throw new ChocolateyException("An error occurred while running choco.", e);
            }

            FormattedOutput = FormatChocoOut(Output);
        }

        /// <exception cref="ChocolateyException"></exception>
        public void Execute()
        {
            // TODO potentially redirect output and expose events

            ProcessStartInfo procInfo = new ProcessStartInfo(CHOCO_BIN, Args)
            {
                UseShellExecute = false,
            };

            try
            {
                Process proc = Process.Start(procInfo);
                proc.WaitForExit();

                if (proc.ExitCode != 0)
                {
                    throw new ChocolateyException($"choco did not exit cleanly. Returned {proc.ExitCode}.");
                }
            }
            catch (Win32Exception e)
            {
                throw new ChocolateyException("An error occurred while running choco.", e);
            }
        }

        private List<List<string>> FormatChocoOut(string output)
        {
            List<List<string>> rtn = new List<List<string>>();

            // parse head
            Queue<string> outputLines = new Queue<string>(output.Split("\r\n"));
            if (outputLines.Count > 0)
            {
                if (!outputLines.Dequeue().StartsWith("Chocolatey v"))
                {
                    // TOOD version checks? "Chocolatey v0.10.15"
                }

                // divide out into blocks seperated by empty line
                List<string> currentBlock = new List<string>();
                while (outputLines.Count > 0)
                {
                    string line = outputLines.Dequeue();
                    if (String.Empty.Equals(line))
                    {
                        rtn.Add(currentBlock);
                        currentBlock = new List<string>();
                    }
                    else
                    {
                        currentBlock.Add(line);
                    }
                }
            }

            return rtn;
        }
    }
}
