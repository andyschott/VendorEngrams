using System;

namespace VendorEngrams
{
    public class Vendor
    {
        public int Id { get; set; }
        public bool Display { get; set; }
        public DropStatus Drop { get; set; }
        public string Shorthand { get; set; }
        public uint Interval { get; set; }
        public DateTime NextRefresh { get; set; }
    }
}