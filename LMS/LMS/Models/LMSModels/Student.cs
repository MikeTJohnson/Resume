using System;
using System.Collections.Generic;

namespace LMS.Models.LMSModels
{
    public partial class Student
    {
        public Student()
        {
            Enrollments = new HashSet<Enrollment>();
            Submissions = new HashSet<Submission>();
        }

        public string UId { get; set; } = null!;
        public string? FName { get; set; }
        public string? LName { get; set; }
        public DateOnly? Dob { get; set; }
        public string DAbr { get; set; } = null!;

        public virtual Department DAbrNavigation { get; set; } = null!;
        public virtual ICollection<Enrollment> Enrollments { get; set; }
        public virtual ICollection<Submission> Submissions { get; set; }
    }
}
