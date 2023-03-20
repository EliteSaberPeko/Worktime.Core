using NUnit.Framework;
using System;
using System.Linq;
using Worktime.Core.CRUD;
using Worktime.Core.Models;

namespace Worktime.Tests.DatabaseTests
{
    internal class Line
    {
        [SetUp]
        public void InitDb()
        {
            var user = new WTUser { Id = Guid.NewGuid() };
            var task = new WTTask()
            {
                Name = "First",
                Description = "Desc",
                Completed = false,
                TotalTime = 100,
                WTUserId = user.Id,
                User = user
            };
            var line = new WTLine()
            {
                Task = task,
                WTTaskId = task.Id
            };
            var db = Database.GetMemoryContext();
            db.Database.EnsureDeleted();
            db.Users.Add(user);
            db.Tasks.Add(task);
            db.Lines.Add(line);
            db.SaveChanges();
        }
        [Test]
        public void CanCreate()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            DateTime start = DateTime.Now,
                end = DateTime.Now.AddHours(1);
            double time = (end - start).Minutes / 60;
            var line = new WTLine()
            {
                Date = DateTime.Now,
                BeginTime = DateTime.Now,
                EndTime = DateTime.Now.AddHours(1),
                Time = time,
                Task = task,
                WTTaskId = task.Id
            };
            var processor = new LineProcessor(db);
            processor.Create(line);
            db.SaveChanges();
        }
        [Test]
        public void CreateEmpty()
        {
            var line = new WTLine();
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Create(line));
            Assert.Throws<ArgumentException>(() => processor.Create(new WTLine { WTTaskId = 0 }));
        }
        [Test]
        public void CanUpdate()
        {
            var db = Database.GetMemoryContext();
            var line = db.Lines.First();
            line.BeginTime = DateTime.Now;
            var processor = new LineProcessor(db);
            processor.Update(line);
            db.SaveChanges();
        }
        [Test]
        public void UpdateInvalid()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);

            var line = new WTLine();
            Assert.Throws<ArgumentException>(() => processor.Update(line));

            line = db.Lines.First();
            line.WTTaskId = 0;
            Assert.Throws<ArgumentException>(() => processor.Update(line));
        }
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var line = db.Lines.First();
            var processor = new LineProcessor(db);
            processor.Delete(line.Id);
            db.SaveChanges();
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Delete(0));
        }
    }
}
