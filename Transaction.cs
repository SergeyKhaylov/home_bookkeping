using System;

namespace Homebookkeping
{
    internal class Transaction
    {
        public int id { get; set; }
        public string? type { get; set; }
        public string? category { get; set; }
        public int price { get; set; }
        public string? comment { get; set; }
        public DateOnly adding_date { get; set; }
    }
}
