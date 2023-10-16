using LMS.Controllers;
using LMS.Models.LMSModels;
using LMS_CustomIdentity.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit.Abstractions;

namespace LMSControllerTests
{
    public class UnitTest1
    {
        private readonly ITestOutputHelper output;
        public UnitTest1(ITestOutputHelper output)
        {
            this.output = output;
        }
        // Uncomment the methods below after scaffolding
        // (they won't compile until then)

        [Fact]
        public void Test1()
        {
            // An example of a simple unit test on the CommonController
            CommonController commonCtrl = new CommonController(MakeTinyDB());
            AdministratorController adminCtrl = new AdministratorController(MakeTinyDB());

            // Assert that there are 2 departments initially
            var allDepts = commonCtrl.GetDepartments() as JsonResult;
            dynamic x = allDepts.Value;
            Assert.Equal(2, x.Length);
            Assert.Equal("CS", x[0].subject);

            // Add a new department and assert that it was successfully added
            adminCtrl.CreateDepartment("EE", "Electrical Engineering");
            allDepts = commonCtrl.GetDepartments() as JsonResult;
            dynamic depts = allDepts.Value;
            Assert.Equal(3, depts.Length);
            Assert.Equal("EE", depts[2].subject);

        }


            /// <summary>
            /// Make a very tiny in-memory database, containing just one department
            /// and nothing else.
            /// </summary>
            /// <returns></returns>
            LMSContext MakeTinyDB()
        {
            var contextOptions = new DbContextOptionsBuilder<LMSContext>()
            .UseInMemoryDatabase("LMSControllerTest")
            .ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .UseApplicationServiceProvider(NewServiceProvider())
            .Options;

            var db = new LMSContext(contextOptions);

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            // Add two departments: Computer Science (CS) and Chemical Engineering (CHE)
            db.Departments.Add(new Department { DName = "Computer Science", DAbr = "CS" });
            db.Departments.Add(new Department { DName = "Chemical Engeering", DAbr = "CHE" });

            // Add three professors: Sidney Crosby, Lionel Messi, Kobe Bryant 
            db.Professors.Add(new Professor
            {
                UId = "u2000000",
                FName = "Sidney",
                LName = "Crosby",
                Dob = DateOnly.Parse("01/01/1990"),
                DAbr = "CS"
            });

            db.Professors.Add(new Professor
            {
                UId = "u2000001",
                FName = "Lionel",
                LName = "Messi",
                Dob = DateOnly.Parse("01/01/1990"),
                DAbr = "CS"
            });

            db.Professors.Add(new Professor
            {
                UId = "u2000002",
                FName = "Kobe",
                LName = "Byrant",
                Dob = DateOnly.Parse("01/01/1990"),
                DAbr = "CHE"
            });

            db.SaveChanges();

            return db;
        }

        private static ServiceProvider NewServiceProvider()
        {
            var serviceProvider = new ServiceCollection()
          .AddEntityFrameworkInMemoryDatabase()
          .BuildServiceProvider();

            return serviceProvider;
        }

    }
}