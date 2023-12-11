using System;
namespace ZDDController.DataClasses
{
    public class Sort
    {
        public int sID { get; set; }
        public string pNum { get; set; }
        public DateTime recDate { get; set; }
        public DateTime ttfDate { get; set; }
        public DateTime finDate { get; set; }
        public DateTime workStart { get; set; }
        public DateTime workEnd { get; set; }
        public TimeSpan totalTime { get; set; }
        public bool ttf { get; set; }
        public int qty { get; set; }
        public int dQty { get; set; }
        public double ppm { get; set; }
        public bool bwi { get; set; }
        public bool oring { get; set; }
        public bool special { get; set; }
    }
}

