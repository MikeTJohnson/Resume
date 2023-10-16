using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Course
    {
        public Course()
        {
            Classes = new HashSet<Class>();
        }

        public uint CId { get; set; }
        public uint Num { get; set; }
        public string CName { get; set; } = null!;
        public string DAbr { get; set; } = null!;

        public virtual Department DAbrNavigation { get; set; } = null!;
        public virtual ICollection<Class> Classes { get; set; }
    }
}
