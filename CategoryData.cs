using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homebookkeping
{
    public class CategoryData
    {
        public string Month { get; set; }
        public Dictionary<string, string?> CategoryAmounts { get; set; } = new Dictionary<string, string?>();
    }
}
