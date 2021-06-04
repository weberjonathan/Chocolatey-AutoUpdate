using System;
using System.Collections.Generic;
using System.Text;

namespace CandyShop.Chocolatey
{
    // create instance in caller [CandyShopForm]
    // supply data through ChocolateyWrapper.Upgrade(packages, monitor)
    // - this needs current package progress (%), currentPackageIndex (?)
    // forward data to caller
    // - caller needs performStep events for current and total
    
    public class PackageProgressChangedEventArgs : EventArgs
    {
        public int Value { get; set; }
    }


    public class ChocolateyUpgradeMonitor
    {
        public event EventHandler<PackageProgressChangedEventArgs> PackageProgressChanged;

        public event EventHandler PackageFinished;

        public int TotalPackageCount { get; set; } = 0;

        private int _FinishedPackageCount = 0;
        public int FinishedPackageCount
        {
            get
            {
                return _FinishedPackageCount;
            }
            set
            {
                _FinishedPackageCount = value;
                PackageFinished?.Invoke(this, new EventArgs());
            }
        }

        private int _PackageProgress = 0;
        public int PackageProgress
        {
            get
            {
                return _PackageProgress;
            }
            set
            {
                _PackageProgress = value;
                PackageProgressChangedEventArgs e = new PackageProgressChangedEventArgs
                {
                    Value = value
                };
                PackageProgressChanged?.Invoke(this, e);
            }
        }
    }
}
