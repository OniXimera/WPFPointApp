using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WPFPointApp.Configuration
{
    internal class AppConfiguration
    {
        public const double PlaneMin = -10;
        public const double PlaneMax = 10;
        public const double SizePoint = 6;
        public const double HalfSizePoint = SizePoint / 2;
        public const double PlaneRange = PlaneMax - PlaneMin;
        public const string FileExtension = ".save";
    }
}
