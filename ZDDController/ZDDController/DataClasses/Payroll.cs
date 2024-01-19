using System;
namespace ZDDController.DataClasses
{
	public class Payroll
	{
        public int eID { get; set; }
        public string eName { get; set; }
        public DateTime startTime { get; set; }
        public DateTime endTime { get; set; }
        public TimeSpan time { get; set; }
    }
}

