using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Submission
    {
        public DateTime SubmitTime { get; set; }
        public string SubmitContents { get; set; } = null!;
        public uint Score { get; set; }
        public string UId { get; set; } = null!;
        public uint AssId { get; set; }

        public virtual Assignment Ass { get; set; } = null!;
        public virtual Student UIdNavigation { get; set; } = null!;
    }
}
