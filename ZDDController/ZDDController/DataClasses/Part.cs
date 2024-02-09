using System;
namespace ZDDController.DataClasses
{
	public class Part
	{
        public string pNum { get; set; }
        public double ppm { get; set; }
        public double rate { get; set; }
        public double ttfRate { get; set; }
        public bool bwi { get; set; }
        public bool oring { get; set; }
        public bool special { get; set; }
    }
}

