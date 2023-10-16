using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LMSControllerTests")]

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS.Controllers
{
    [Authorize(Roles = "Student")]
    public class StudentController : Controller
    {
        private LMSContext db;
        public StudentController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Catalog()
        {
            return View();
        }

        public IActionResult Class(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult Assignment(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }


        public IActionResult ClassListings(string subject, string num)
        {
            System.Diagnostics.Debug.WriteLine(subject + num);
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }


        /*******Begin code to modify********/

        /// <summary>
        /// Returns a JSON array of the classes the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester
        /// "year" - The year part of the semester
        /// "grade" - The grade earned in the class, or "--" if one hasn't been assigned
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = (from e in db.Enrollments
                        where e.UId == uid
                        select new
                        {
                            subject = e.Class.CIdNavigation.DAbr,
                            number = e.Class.CIdNavigation.Num,
                            name = e.Class.CIdNavigation.CName,
                            season = e.Class.Season,
                            year = e.Class.Year,
                            grade = e.Grade
                        }).ToArray();
            return Json(query);
        }

        /// <summary>
        /// Returns a JSON array of all the assignments in the given class that the given student is enrolled in.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The category name that the assignment belongs to
        /// "due" - The due Date/Time
        /// "score" - The score earned by the student, or null if the student has not submitted to this assignment.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="uid"></param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInClass(string subject, int num, string season, int year, string uid)
        {
            var assignQuery = from a in db.Assignments
                              where a.Ac.Class.CIdNavigation.DAbr == subject
                              && a.Ac.Class.CIdNavigation.Num == num
                              && a.Ac.Class.Season == season
                              && a.Ac.Class.Year == year
                              select new {
                                  acID = a.AcId,
                                  catName = a.Ac.Name,
                                  assignID = a.AssId,
                                  assignName = a.Name,
                                  dueDate = a.DueDate
                              };

            // Query the submissions table for the assignments the student has submitted
            var subQuery = (from q in assignQuery
                           join s in db.Submissions
                           on new { A = q.assignID, B = uid }
                           equals new { A = s.AssId, B = s.UId }
                           into join1
                           from j1 in join1.DefaultIfEmpty()
                           select new
                           {
                               cname = q.catName,
                               aname = q.assignName,
                               due = q.dueDate,
                               score = j1 == null ? null : (uint?)j1.Score
                           }).ToArray();

            return Json(subQuery);
        }

        /// <summary>
        /// Adds a submission to the given assignment for the given student
        /// The submission should use the current time as its DateTime
        /// You can get the current time with DateTime.Now
        /// The score of the submission should start as 0 until a Professor grades it
        /// If a Student submits to an assignment again, it should replace the submission contents
        /// and the submission time (the score should remain the same).
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="uid">The student submitting the assignment</param>
        /// <param name="contents">The text contents of the student's submission</param>
        /// <returns>A JSON object containing {success = true/false}</returns>
        public IActionResult SubmitAssignmentText(string subject, int num, string season, int year,
          string category, string asgname, string uid, string contents)
        {
            var assidQuery = (from cl in db.Classes
                         join ac in db.AssignmentCategories
                         on cl.ClassId equals ac.ClassId
                         into join1
                         from j1 in join1
                         join a in db.Assignments
                         on j1.AcId equals a.AcId
                         where cl.CIdNavigation.DAbr == subject
                         && cl.CIdNavigation.Num == num
                         && cl.Season == season
                         && cl.Year == year
                         && j1.Name == category
                         && a.Name == asgname
                         select a.AssId).SingleOrDefault();

            // Query the database to determine if the student has previously submitted the assignemnt 
            var subQuery = (from s in db.Submissions
                           where s.AssId == assidQuery
                           && s.UId == uid
                           select s).SingleOrDefault();
            try
            {
                Submission sub;
                // If the submission query is not null, update the submission contents and time
                if (subQuery is not null)
                {
                    sub = subQuery;
                    sub.SubmitContents = contents;
                    sub.SubmitTime = DateTime.Now;
                }
                // Otherwise create a new submission and add it to the database
                else
                {
                    sub = new()
                    {
                        SubmitTime = DateTime.Now,
                        SubmitContents = contents,
                        Score = 0,
                        UId = uid,
                        AssId = assidQuery
                    };
                    db.Submissions.Add(sub);
                }
                db.SaveChanges();
                return Json(new { success = true });
            }
            // If an exception is thrown, return failure messge
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Enrolls a student in a class.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing {success = {true/false}. 
        /// false if the student is already enrolled in the class, true otherwise.</returns>
        public IActionResult Enroll(string subject, int num, string season, int year, string uid)
        {
            // Query the database for the class ID
            var classQuery = (from cl in db.Classes
                              where cl.CIdNavigation.DAbr == subject
                              && cl.CIdNavigation.Num == num
                              && cl.Season == season
                              && cl.Year == year
                              select cl.ClassId).SingleOrDefault();

            // Create new enrollment object 
            Enrollment e = new()
            {
                UId = uid,
                ClassId = classQuery,
                Grade = "--"
            };

            // Add the enrollment object to the database and send success messgae
            try {
                db.Enrollments.Add(e);
                db.SaveChanges();
                return Json(new { success = true });
            }
            // If an exception occurs, failue message is sent
            catch(Exception) {
                return Json(new { success = false });
            } 
        }

        /// <summary>
        /// Calculates a student's GPA
        /// A student's GPA is determined by the grade-point representation of the average grade in all their classes.
        /// Assume all classes are 4 credit hours.
        /// If a student does not have a grade in a class ("--"), that class is not counted in the average.
        /// If a student is not enrolled in any classes, they have a GPA of 0.0.
        /// Otherwise, the point-value of a letter grade is determined by the table on this page:
        /// https://advising.utah.edu/academic-standards/gpa-calculator-new.php
        /// </summary>
        /// <param name="uid">The uid of the student</param>
        /// <returns>A JSON object containing a single field called "gpa" with the number value</returns>
        public IActionResult GetGPA(string uid)
        {
            // Query Enrollemnts table for a student's grades
            var gradesQuery = from e in db.Enrollments
                              where e.UId == uid
                              select e.Grade;

            double points = 0.0;
            double numClasses = 0.0;
            double GPA = 0.0;
            if (gradesQuery != null)
            {
                foreach (var grade in gradesQuery)
                {
                    // Only calculate GPA using classes with a grade
                    if (grade != "--")
                    {
                        // Determine how many points to add based on letter grade
                        if (grade == "A")
                            points += 4.0;
                        else if (grade == "A-")
                            points += 3.7;
                        else if (grade == "B+")
                            points += 3.3;
                        else if (grade == "B")
                            points += 3.0;
                        else if (grade == "B-")
                            points += 2.7;
                        else if (grade == "C+")
                            points += 2.3;
                        else if (grade == "C")
                            points += 2.0;
                        else if (grade == "C-")
                            points += 1.7;
                        else if (grade == "D+")
                            points += 1.3;
                        else if (grade == "D")
                            points += 1.0;
                        else if (grade == "D-")
                            points += 0.7;

                        // Increease the number of classes counted towards the GPA
                        numClasses += 1;
                    }
                }
                // Calculate the GPA
                if (numClasses != 0) {
                    GPA = points / numClasses;
                }
            }
            return Json(new {gpa = GPA });
        }
                
        /*******End code to modify********/

    }
}

