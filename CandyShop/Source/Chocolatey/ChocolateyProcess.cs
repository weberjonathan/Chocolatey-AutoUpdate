using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;

namespace CandyShop.Chocolatey
{
    public class ChocolateyProcess
    {
        private const string CHOCO_BIN = "choco";

        private string Args;

        public ChocolateyProcess(string args)
        {
            Args = args;
        }

        public event DataReceivedEventHandler OutputDataReceived;

        public List<string> Output { get; private set; } = new List<string>();

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

                // TODO testing!!!
                proc.BeginOutputReadLine();
                proc.OutputDataReceived += new DataReceivedEventHandler((sender, e) =>
                {
                    OutputDataReceived?.Invoke(this, e);
                    if (e.Data != null)
                    {
                        Output.Add(e.Data);
                        // if e.Data is null stream was closed
                    }

                });
                // ---

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

        private List<List<string>> FormatChocoOut(List<string> output)
        {
            List<List<string>> rtn = new List<List<string>>();

            // parse head
            Queue<string> outputLines = new Queue<string>(output);
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

                if (currentBlock.Count > 0)
                {
                    rtn.Add(currentBlock);
                }
            }

            return rtn;
        }
    }
}
