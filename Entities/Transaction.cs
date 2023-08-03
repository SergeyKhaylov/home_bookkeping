using System;

namespace Homebookkeping
{
    internal class Transaction
    {
        public int id { get; set; }
        public string? type { get; set; }
        public int category_id { get; set; }
        public double price { get; set; }
        public string? comment { get; set; }
        public DateOnly adding_date { get; set; }
        public int user_id { get; set; }
    }
}
