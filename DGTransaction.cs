﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homebookkeping
{
    class DGTransaction
    {
        public int id {  get; set; }
        public string? type { get; set; }
        public string? category { get; set; }
        public double price { get; set; }
        public string? comment { get; set; }
        public string? adding_date { get; set; }
    }
}
