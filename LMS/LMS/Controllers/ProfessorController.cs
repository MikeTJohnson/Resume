using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS_CustomIdentity.Controllers
{
    [Authorize(Roles = "Professor")]
    public class ProfessorController : Controller
    {

        private readonly LMSContext db;

        public ProfessorController(LMSContext _db)
        {
            db = _db;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Students(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
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

        public IActionResult Categories(string subject, string num, string season, string year)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            return View();
        }

        public IActionResult CatAssignments(string subject, string num, string season, string year, string cat)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
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

        public IActionResult Submissions(string subject, string num, string season, string year, string cat, string aname)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            return View();
        }

        public IActionResult Grade(string subject, string num, string season, string year, string cat, string aname, string uid)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            ViewData["season"] = season;
            ViewData["year"] = year;
            ViewData["cat"] = cat;
            ViewData["aname"] = aname;
            ViewData["uid"] = uid;
            return View();
        }

        /*******Begin code to modify********/


        /// <summary>
        /// Returns a JSON array of all the students in a class.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "dob" - date of birth
        /// "grade" - the student's grade in this class
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetStudentsInClass(string subject, int num, string season, int year)
        {
            var query = from co in db.Courses
                        join cl in db.Classes
                        on co.CId equals cl.CId
                        into join1
                        from j1 in join1
                        join e in db.Enrollments
                        on j1.ClassId equals e.ClassId
                        into join2
                        from j2 in join2
                        join s in db.Students
                        on j2.UId equals s.UId
                        where co.DAbr == subject
                        && co.Num == num
                        && j1.Season == season
                        && j1.Year == year
                        select new
                        {
                            fname = s.FName,
                            lname = s.LName,
                            uid = s.UId,
                            dob =s.Dob,
                            grade = j2.Grade
                        };
            return Json(query);
        }



        /// <summary>
        /// Returns a JSON array with all the assignments in an assignment category for a class.
        /// If the "category" parameter is null, return all assignments in the class.
        /// Each object in the array should have the following fields:
        /// "aname" - The assignment name
        /// "cname" - The assignment category name.
        /// "due" - The due DateTime
        /// "submissions" - The number of submissions to the assignment
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class, 
        /// or null to return assignments from all categories</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentsInCategory(string subject, int num, string season, int year, string category)
        {
            if (category == null)
            {
                var query = from a in db.Assignments
                            where a.Ac.Class.Year == year
                            && a.Ac.Class.Season == season
                            && a.Ac.Class.CIdNavigation.Num == num
                            && a.Ac.Class.CIdNavigation.DAbr == subject
                            select new
                            {
                                aname = a.Name,
                                due = a.DueDate,
                                submissions = a.Submissions.Count(),
                                cname = a.Ac.Name
                            };
                return Json(query);
            }
            else
            {
                var query = from a in db.Assignments
                            where a.Ac.Class.Year == year
                            && a.Ac.Class.Season == season
                            && a.Ac.Class.CIdNavigation.Num == num
                            && a.Ac.Class.CIdNavigation.DAbr == subject
                            && a.Ac.Name == category
                            select new
                            {
                                aname = a.Name,
                                due = a.DueDate,
                                submissions = a.Submissions.Count(),
                                cname = a.Ac.Name
                            };
                return Json(query);
            }
        }


        /// <summary>
        /// Returns a JSON array of the assignment categories for a certain class.
        /// Each object in the array should have the folling fields:
        /// "name" - The category name
        /// "weight" - The category weight
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetAssignmentCategories(string subject, int num, string season, int year)
        {
            var query = from a in db.AssignmentCategories
                        where a.Class.Year == year
                        && a.Class.Season == season
                        && a.Class.CIdNavigation.Num == num
                        && a.Class.CIdNavigation.DAbr == subject
                        select new
                        {
                            name = a.Name,
                            weight = a.Weight
                        };
            return Json(query);
        }

        /// <summary>
        /// Creates a new assignment category for the specified class.
        /// If a category of the given class with the given name already exists, return success = false.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The new category name</param>
        /// <param name="catweight">The new category weight</param>
        /// <returns>A JSON object containing {success = true/false} </returns>
        public IActionResult CreateAssignmentCategory(string subject, int num, string season, int year, string category, int catweight)
        {
            try
            {
                //check if the category already exists in the class
                var acQuery = from ac in db.AssignmentCategories
                              where ac.Class.Year == year
                              && ac.Class.Season == season
                              && ac.Class.CIdNavigation.DAbr == subject
                              && ac.Class.CIdNavigation.Num == num
                              select ac.Name;
                foreach (var row in acQuery)
                {
                    if (category == row.ToString())
                    {
                        return Json(new { success = false });
                    }
                }

                //get the classID for the specific class using the subject, num, season, and year parameter
                var classIDQuery = (from c in db.Classes
                                    where c.Year == year
                                    && c.Season == season
                                    && c.CIdNavigation.DAbr == subject
                                    && c.CIdNavigation.Num == num
                                    select c.ClassId).SingleOrDefault();

                //add the assignment category to the database
                AssignmentCategory a = new()
                {
                    Name = category,
                    Weight = (uint)catweight,
                    ClassId = classIDQuery
                };
                db.AssignmentCategories.Add(a);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }

        /// <summary>
        /// Creates a new assignment for the given class and category.
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The new assignment name</param>
        /// <param name="asgpoints">The max point value for the new assignment</param>
        /// <param name="asgdue">The due DateTime for the new assignment</param>
        /// <param name="asgcontents">The contents of the new assignment</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult CreateAssignment(string subject, int num, string season, int year, string category, string asgname, int asgpoints, DateTime asgdue, string asgcontents)
        {
            //get the assignment category ID for the assignment
            var acIDQuery = (from a in db.AssignmentCategories
                             where a.Name == category
                             && a.Class.Year == year
                             && a.Class.Season == season
                             && a.Class.CIdNavigation.Num == num
                             && a.Class.CIdNavigation.DAbr == subject
                             select a.AcId).SingleOrDefault();

            //create the assignment
            Assignment ass = new()
            {
                Name = asgname,
                Points = (uint)asgpoints,
                DueDate = asgdue,
                Contents = asgcontents,
                AcId = acIDQuery
            };
            try
            {
                db.Assignments.Add(ass);
                db.SaveChanges();

                //get the UID's of the students in the class
                var students = (from e in db.Enrollments
                               where e.Class.Year == year
                               && e.Class.Season == season
                               && e.Class.CIdNavigation.Num == num
                               && e.Class.CIdNavigation.DAbr == subject
                               select e.UId).ToArray();

                //change the grade of each student
                foreach(var student in students)
                {
                    UpdateGrade(subject, num, season, year, student);
                }
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Gets a JSON array of all the submissions to a certain assignment.
        /// Each object in the array should have the following fields:
        /// "fname" - first name
        /// "lname" - last name
        /// "uid" - user ID
        /// "time" - DateTime of the submission
        /// "score" - The score given to the submission
        /// 
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetSubmissionsToAssignment(string subject, int num, string season, int year, string category, string asgname)
        {
            var query = from s in db.Submissions
                        where s.Ass.Name == asgname
                        && s.Ass.Ac.Name == category
                        && s.Ass.Ac.Class.Year == year
                        && s.Ass.Ac.Class.Season == season
                        && s.Ass.Ac.Class.CIdNavigation.Num == num
                        && s.Ass.Ac.Class.CIdNavigation.DAbr == subject
                        select new
                        {
                            fname = s.UIdNavigation.FName,
                            lname = s.UIdNavigation.LName,
                            uid = s.UId,
                            time = s.SubmitTime,
                            score = s.Score
                        };
            return Json(query);
        }


        /// <summary>
        /// Set the score of an assignment submission
        /// </summary>
        /// <param name="subject">The course subject abbreviation</param>
        /// <param name="num">The course number</param>
        /// <param name="season">The season part of the semester for the class the assignment belongs to</param>
        /// <param name="year">The year part of the semester for the class the assignment belongs to</param>
        /// <param name="category">The name of the assignment category in the class</param>
        /// <param name="asgname">The name of the assignment</param>
        /// <param name="uid">The uid of the student who's submission is being graded</param>
        /// <param name="score">The new score for the submission</param>
        /// <returns>A JSON object containing success = true/false</returns>
        public IActionResult GradeSubmission(string subject, int num, string season, int year, string category, string asgname, string uid, int score)
        {
            //get the submission
            var subQuery = (from s in db.Submissions
                           where s.UId == uid
                           && s.Ass.Name == asgname
                           && s.Ass.Ac.Name == category
                           && s.Ass.Ac.Class.Year == year
                           && s.Ass.Ac.Class.Season == season
                           && s.Ass.Ac.Class.CIdNavigation.Num == num
                           && s.Ass.Ac.Class.CIdNavigation.DAbr == subject
                           select s).SingleOrDefault();

            //begin try block
            try
            {
                //change the submission score
                subQuery.Score = (uint)score;
                System.Diagnostics.Debug.WriteLine("SCORE: " + subQuery.Score);
                db.SaveChanges();
                UpdateGrade(subject, num, season, year, uid);
                return Json(new { success = true });
            }
            catch(Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Returns a JSON array of the classes taught by the specified professor
        /// Each object in the array should have the following fields:
        /// "subject" - The subject abbreviation of the class (such as "CS")
        /// "number" - The course number (such as 5530)
        /// "name" - The course name
        /// "season" - The season part of the semester in which the class is taught
        /// "year" - The year part of the semester in which the class is taught
        /// </summary>
        /// <param name="uid">The professor's uid</param>
        /// <returns>The JSON array</returns>
        public IActionResult GetMyClasses(string uid)
        {
            var query = from c in db.Classes
                        where c.Prof == uid
                        select new
                        {
                            year = c.Year,
                            season = c.Season,
                            subject = c.CIdNavigation.DAbr,
                            number = c.CIdNavigation.Num,
                            name = c.CIdNavigation.CName,
                        };
            return Json(query);
        }

        private void UpdateGrade(string subject, int num, string season, int year, string uid) {
            // Get all the assignment categories for class
            var allAC = (from ac in db.AssignmentCategories
                        where ac.Class.Year == year
                        && ac.Class.Season == season
                        && ac.Class.CIdNavigation.DAbr == subject
                        && ac.Class.CIdNavigation.Num == num
                        select new {
                            acID = ac.AcId,
                            acWeight = ac.Weight
                        }).ToArray();
            var allAssignCats = allAC.AsEnumerable().Select(i => i.acID).ToArray();

            // Get all assignments for a class
            var allAssign = (from a in db.Assignments
                            where allAssignCats.Contains(a.AcId)
                            select new
                            {
                                acID = a.AcId,
                                points = a.Points,
                                assID = a.AssId,
                                acWeight = a.Ac.Weight
                            }).ToArray();

            // Get array of all assignment ids in the class
            var allAssignIds = allAssign.AsEnumerable().Select(i => i.assID).ToArray();

            // Get the student's scores 
            var allSubs = (from s in db.Submissions
                          where s.UId == uid
                          && allAssignIds.Contains(s.AssId)
                          select new
                          {
                              assID = s.AssId,
                              score = s.Score
                          }).ToArray();

            double weightedPoints = 0.0;
            double totalWeight = 0.0;

            // Iterate through each assignment category
            foreach(var ac in allAC) {
                double totalPoints = 0.0;
                double studentPoints = 0.0;
                // Find the assignments in the category and add to the category's total points and the student's points
                foreach(var a in allAssign) {
                    if (a.acID == ac.acID) {
                        totalPoints += a.points;
                        // Find the student's score if they have submitted an assignment
                        foreach(var s in allSubs) {
                            if (s.assID == a.assID) {
                                studentPoints += s.score;
                                break;
                            }
                        }
                    }
                }
                // If there are no assignments in the category, do not include it
                if (totalPoints > 0.0)
                {
                    weightedPoints += (studentPoints / totalPoints) * ac.acWeight;
                    totalWeight += ac.acWeight;
                }
            }

            // Calculate the scoring factor and final percentage
            double scalingFactor = 100 / totalWeight;
            double studentPercentage = weightedPoints * scalingFactor;

            //  Turn the total percentage into a letter grade
            string letterGrade = "";
            if (studentPercentage >= 93.0)
                letterGrade = "A";
            else if (studentPercentage >= 90.0)
                letterGrade = "A-";
            else if (studentPercentage >= 87.0)
                letterGrade = "B+";
            else if (studentPercentage >= 83.0)
                letterGrade = "B";
            else if (studentPercentage >= 80.0)
                letterGrade = "B-";
            else if (studentPercentage >= 77.0)
                letterGrade = "C+";
            else if (studentPercentage >= 73.0)
                letterGrade = "C";
            else if (studentPercentage >= 70.0)
                letterGrade = "C-";
            else if (studentPercentage >= 67.0)
                letterGrade = "D+";
            else if (studentPercentage >= 63.0)
                letterGrade = "D";
            else
                letterGrade = "D-";

            //update the students grade
            var Grade = (from e in db.Enrollments
                         where e.UId == uid
                         && e.Class.Year == year
                         && e.Class.Season == season
                         && e.Class.CIdNavigation.Num == num
                         && e.Class.CIdNavigation.DAbr == subject
                         select e).SingleOrDefault();
            try
            {
                Grade.Grade = letterGrade;
                db.SaveChanges();
            }
            catch (Exception)
            {
                return;
            }
        }
        /*******End code to modify********/
    }
}

