using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktime.Core;
using Worktime.Core.Models;
using Worktime.Tests.DatabaseTests;

namespace Worktime.Tests
{
    public class RowsTests
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
                Identifier = "First",
                Title = "Desc",
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
            for (int i = 1; i <= 12; i++)
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
            Rows rows = new(processor);
            var today = rows.GetToday(userId);
            Assert.AreEqual(2, today.Count());
        }
        [Test]
        public void GetOn15_03_2023()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            Rows rows = new(processor);
            var today = rows.GetOnDate(userId, new DateTime(2023, 3, 15));
            Assert.AreEqual(1, today.Count());
        }
        [Test]
        public void GetFromTask()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var userId = db.Users.First().Id;
            var taskId = db.Tasks.First().Id;
            Rows rows = new(processor);
            var today = rows.GetFromTask(userId, taskId);
            Assert.AreEqual(29, today.Count());
        }
    }
}
