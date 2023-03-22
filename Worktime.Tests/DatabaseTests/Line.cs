using NUnit.Framework;
using System;
using System.Collections.Generic;
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
                Date = new DateTime(2023, 3, 20).ToUniversalTime(),
                Task = task,
                WTTaskId = task.Id
            };
            var db = Database.GetMemoryContext();
            db.Database.EnsureDeleted();
            db.Users.Add(user);
            db.Tasks.Add(task);
            db.Lines.Add(line);
            line = new WTLine()
            {
                Date = new DateTime(2023, 3, 21).ToUniversalTime(),
                Task = task,
                WTTaskId = task.Id
            };
            db.Lines.Add(line);
            db.SaveChanges();
        }

        #region Create
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
            var result = processor.Create(line);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanCreateMany()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            List<WTLine> lines = new();
            for (int i = 0; i < 10; i++)
            {
                DateTime start = DateTime.Now.AddHours(i),
                    end = DateTime.Now.AddHours(i + 1);
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
                lines.Add(line);
            }

            var processor = new LineProcessor(db);
            var result = processor.Create(lines);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CreateInvalid()
        {
            var line = new WTLine();
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);
            var result = processor.Create(line);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);

            result = processor.Create(new WTLine { WTTaskId = 0 });
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);
        }
        [Test]
        public void CreateInvalidMany()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            List<WTLine> lines = new();
            for (int i = 0; i < 4; i++)
            {
                DateTime start = DateTime.Now.AddHours(i),
                    end = DateTime.Now.AddHours(i + 1);
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
                lines.Add(line);
            }
            lines.Add(new WTLine());
            var processor = new LineProcessor(db);
            var result = processor.Create(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 4", result.Message);

            lines = lines.Prepend(new WTLine()).ToList();
            result = processor.Create(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 0", result.Message);
        }
        #endregion

        #region Read
        [Test]
        public void CanRead()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            var line = db.Lines.First();
            var processor = new LineProcessor(db);
            var ie = processor.ReadAsIEnumerable(task.Id);
            var iq = processor.ReadAsIQueryable(task.Id);
            Assert.AreEqual(line, ie.First());
            Assert.AreEqual(line.Date, ie.First().Date);
            Assert.AreEqual(new DateTime(2023, 3, 20).ToUniversalTime(), ie.First().Date);

            Assert.AreEqual(line, iq.First());
            Assert.AreEqual(line.Date, iq.First().Date);
            Assert.AreEqual(new DateTime(2023, 3, 20).ToUniversalTime(), iq.First().Date);
        } 
        #endregion

        #region Update
        [Test]
        public void CanUpdate()
        {
            var db = Database.GetMemoryContext();
            var line = db.Lines.First();
            line.BeginTime = DateTime.Now;
            var processor = new LineProcessor(db);
            var result = processor.Update(line);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanUpdateMany()
        {
            var db = Database.GetMemoryContext();
            var lines = db.Lines.ToList();
            int buf = 0;
            foreach (var line in lines)
            {
                line.BeginTime = DateTime.Now.AddHours(buf);
                buf++;
            }

            var processor = new LineProcessor(db);
            var result = processor.Update(lines);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void UpdateInvalid()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);

            var line = new WTLine();
            var result = processor.Update(line);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Line was not found!", result.Message);

            line = db.Lines.First();
            line.WTTaskId = 0;
            result = processor.Update(line);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);
        }
        [Test]
        public void UpdateInvalidMany()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);

            var lines = db.Lines.ToList();
            ulong id = lines[1].Id;
            lines[1].Id = 0;
            var result = processor.Update(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Line was not found! Index: 1", result.Message);

            lines[1].Id = id;
            lines[0].WTTaskId = 0;
            result = processor.Update(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 0", result.Message);
        }
        #endregion

        #region Delete
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var line = db.Lines.First();
            var processor = new LineProcessor(db);
            var result = processor.Delete(line.Id);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanDeleteMany()
        {
            var db = Database.GetMemoryContext();
            var lines = db.Lines.ToList();
            var processor = new LineProcessor(db);
            var result = processor.Delete(lines.Select(x => x.Id));
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);
            var result = processor.Delete(0UL);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Line was not found!", result.Message);
        }
        [Test]
        public void DeleteEmptyMany()
        {
            var db = Database.GetMemoryContext();
            var processor = new LineProcessor(db);
            var lines = db.Lines.Select(x => x.Id).ToList();
            lines.Add(0UL);
            var result = processor.Delete(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Line was not found! Index: 2", result.Message);

            lines = lines.Prepend(0UL).ToList();
            result = processor.Delete(lines);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Line was not found! Index: 0", result.Message);
        } 
        #endregion
    }
}
