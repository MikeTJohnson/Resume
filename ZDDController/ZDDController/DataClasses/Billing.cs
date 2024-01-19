using System;
namespace ZDDController.DataClasses
{
    public class Billing
    {
        public int sID { get; set; }
        public string pNum { get; set; }
        public decimal rate { get; set; }
        public int ttf { get; set; }
        public decimal ttfRate { get; set; }
        public int qty { get; set; }
    }
}

