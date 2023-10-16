using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using LMS.Models.LMSModels;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("LMSControllerTests")]

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace LMS.Controllers
{
    public class AdministratorController : Controller
    {
        private readonly LMSContext db;

        public AdministratorController(LMSContext _db)
        {
            db = _db;
        }

        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Department(string subject)
        {
            ViewData["subject"] = subject;
            return View();
        }

        public IActionResult Course(string subject, string num)
        {
            ViewData["subject"] = subject;
            ViewData["num"] = num;
            return View();
        }

        /*******Begin code to modify********/

        /// <summary>
        /// Create a department which is uniquely identified by it's subject code
        /// </summary>
        /// <param name="subject">the subject code</param>
        /// <param name="name">the full name of the department</param>
        /// <returns>A JSON object containing {success = true/false}.
        /// false if the department already exists, true otherwise.</returns>
        public IActionResult CreateDepartment(string subject, string name)
        {
            Department d = new()
            {
                DAbr = subject,
                DName = name
            };
            try
            {
                db.Departments.Add(d);
                db.SaveChanges();
                return Json(new { success = true });
            }
            catch (Exception)
            {
                return Json(new { success = false });
            }
        }


        /// <summary>
        /// Returns a JSON array of all the courses in the given department.
        /// Each object in the array should have the following fields:
        /// "number" - The course number (as in 5530)
        /// "name" - The course name (as in "Database Systems")
        /// </summary>
        /// <param name="subjCode">The department subject abbreviation (as in "CS")</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetCourses(string subject)
        {
            var query = (from c in db.Courses
                        where c.DAbr == subject
                        select new
                        {
                            number = c.Num,
                            name = c.CName
                        }).ToArray();
            return Json(query);
        }

        /// <summary>
        /// Returns a JSON array of all the professors working in a given department.
        /// Each object in the array should have the following fields:
        /// "lname" - The professor's last name
        /// "fname" - The professor's first name
        /// "uid" - The professor's uid
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <returns>The JSON result</returns>
        public IActionResult GetProfessors(string subject)
        {
            var query = (from p in db.Professors
                        where p.DAbr == subject
                        select new
                        {
                            lname = p.LName,
                            fname = p.FName,
                            uid = p.UId
                        }).ToArray();
            return Json(query);
        }



        /// <summary>
        /// Creates a course.
        /// A course is uniquely identified by its number + the subject to which it belongs
        /// </summary>
        /// <param name="subject">The subject abbreviation for the department in which the course will be added</param>
        /// <param name="number">The course number</param>
        /// <param name="name">The course name</param>
        /// <returns>A JSON object containing {success = true/false}.
        /// false if the course already exists, true otherwise.</returns>
        public IActionResult CreateCourse(string subject, int number, string name)
        {
            Course c = new()
            {
                DAbr = subject,
                Num = (uint)number,
                CName = name
            };
            try {
                db.Courses.Add(c);
                db.SaveChanges();
                return Json(new {success = true});
            }
            catch(Exception) {
                return Json(new { success = false });
            }
        }



        /// <summary>
        /// Creates a class offering of a given course.
        /// </summary>
        /// <param name="subject">The department subject abbreviation</param>
        /// <param name="number">The course number</param>
        /// <param name="season">The season part of the semester</param>
        /// <param name="year">The year part of the semester</param>
        /// <param name="start">The start time</param>
        /// <param name="end">The end time</param>
        /// <param name="location">The location</param>
        /// <param name="instructor">The uid of the professor</param>
        /// <returns>A JSON object containing {success = true/false}. 
        /// false if another class occupies the same location during any time 
        /// within the start-end range in the same semester, or if there is already
        /// a Class offering of the same Course in the same Semester,
        /// true otherwise.</returns>
        public IActionResult CreateClass(string subject, int number, string season, int year, DateTime start, DateTime end, string location, string instructor)
        {
            // Convert start and end parameters to TimeOnly
            TimeOnly start_time = TimeOnly.FromDateTime(start);
            TimeOnly end_time = TimeOnly.FromDateTime(end);

            // Query the database for classes held in the same year and season as the new class
            var query = from cl in db.Classes
                        where cl.Season == season
                        && cl.Year == year
                        select new
                        {
                            dAbr = cl.CIdNavigation.DAbr,
                            num = cl.CIdNavigation.Num,
                            starttime = cl.StartTime,
                            endtime = cl.EndTime,
                            loc = cl.Location
                        };

            // Check all classes held in the same year and season if they are for the same course or during the same time at the same location
            foreach(var row in query) {
                // Return false if a class offering for the same course is already being held during the semester
                if (row.dAbr == subject && row.num == number) {
                    return Json(new { success = false });
                }
                // return false if any class is held at the same time at the same location
                else if ((row.loc == location) && ((start_time <= row.starttime && row.starttime <= end_time) ||
                    (start_time <= row.endtime && row.endtime <= end_time))) {
                    return Json(new { success = false });
                }
            }

            // Query the Courses table in the database to determine the course ID using the departmemnt abbreviation and course number
            var class_cid = (from co in db.Courses
                             where co.DAbr == subject
                             && co.Num == number
                             select co.CId).Single();

            // Create a new class
            Class c = new()
            {
                Season = season,
                Year = (uint)year,
                Location = location,
                StartTime = TimeOnly.FromDateTime(start),
                EndTime = TimeOnly.FromDateTime(end),
                Prof = instructor,
                CId = class_cid
            };

            // If the class is succesfully added to the database, return success message
            try {
                db.Classes.Add(c);
                db.SaveChanges();
                return Json(new { success = true });
            }
            // Otherwise return failure message
            catch(Exception) {
                return Json(new { success = false });
            }
        }


        /*******End code to modify********/

    }
}

