using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktime.Core.Models;
using Worktime.Core;
using Worktime.Tests.DatabaseTests;

namespace Worktime.Tests
{
    public class HoursTests
    {
        [SetUp]
        public void Setup()
        {
            #region Init
            var db = Database.GetMemoryContext();
            db.Database.EnsureDeleted();
            var startup = new Startup(db);

            var user = new WTUser { Id = Guid.NewGuid() };
            startup.Create(user);

            var lines = new List<WTLine>();
            var task = new WTTask()
            {
                Name = "First",
                Description = "Desc",
                Completed = false,
                TotalTime = 100,
                WTUserId = user.Id,
                User = user
            };
            task = startup.Create(task).Items.First();

            for (int i = 0; i < 10; i++)
            {
                DateTime date = new DateTime(2023, 3, 10).Date.AddDays(i);
                lines.Add(new()
                {
                    Date = date.ToUniversalTime(),
                    BeginTime = date.ToUniversalTime(),
                    EndTime = date.AddHours(1).ToUniversalTime(),
                    Task = task,
                    WTTaskId = task.Id
                });
            }
            for (int i = 0; i < 5; i++)
            {
                DateTime date = new DateTime(2023, 1, 10).Date.AddDays(i);
                lines.Add(new()
                {
                    Date = date.ToUniversalTime(),
                    BeginTime = date.ToUniversalTime(),
                    EndTime = date.AddHours(1).ToUniversalTime(),
                    Task = task,
                    WTTaskId = task.Id
                });
            }
            for (int i = 1; i < 13; i++)
            {
                DateTime date = new DateTime(2022, i, 10).Date;
                lines.Add(new()
                {
                    Date = date.ToUniversalTime(),
                    BeginTime = date.ToUniversalTime(),
                    EndTime = date.AddHours(1).ToUniversalTime(),
                    Task = task,
                    WTTaskId = task.Id
                });
            }
            for (int i = 0; i < 2; i++)
            {
                DateTime date = DateTime.Today;
                lines.Add(new()
                {
                    Date = date.ToUniversalTime(),
                    BeginTime = date.ToUniversalTime(),
                    EndTime = date.AddHours(1).ToUniversalTime(),
                    Task = task,
                    WTTaskId = task.Id
                });
            }
            startup.Create(lines);
            #endregion
        }

        [Test]
        public void GetToday()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetOnThisDay(userId);
            Assert.AreEqual(2D, hour);
        }
        [Test]
        public void GetOn15_03_2023()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetOnDay(userId, new DateTime(2023, 3, 15));
            Assert.AreEqual(1D, hour);
        }
        [Test]
        public void GetOnWeek()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetOnWeek(userId, new DateTime(2023, 3, 15));
            Assert.AreEqual(7D, hour);
        }
        [Test]
        public void GetOnMonth()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetOnMonth(userId, new DateTime(2023, 1, 15));
            Assert.AreEqual(5D, hour);
        }
        [Test]
        public void GetOnYear()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetOnYear(userId, new DateTime(2022, 10, 15));
            Assert.AreEqual(12D, hour);
        }
        [Test]
        public void GetFromTask()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            var taskId = db.Tasks.First().Id;
            Hours hours = new(processor);
            var hour = hours.GetFromTask(userId, taskId);
            Assert.AreEqual(29D, hour);
        }
    }
}
