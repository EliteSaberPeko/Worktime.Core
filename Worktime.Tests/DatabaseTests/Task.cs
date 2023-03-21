using NUnit.Framework;
using System;
using System.Linq;
using Worktime.Core.CRUD;
using Worktime.Core.Models;

namespace Worktime.Tests.DatabaseTests
{
    internal class Task
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
            var db = Database.GetMemoryContext();
            db.Database.EnsureDeleted();
            db.Users.Add(user);
            db.Tasks.Add(task);
            db.SaveChanges();
        }
        [Test]
        public void CanCreate()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            var task = new WTTask()
            {
                Name = "First 2",
                Description = "Desc 2",
                Completed = false,
                TotalTime = 110,
                WTUserId = user.Id,
                User = user
            };
            var processor = new TaskProcessor(db);
            processor.Create(task);
            db.SaveChanges();
        }
        [Test]
        public void CreateEmpty()
        {
            var task = new WTTask();
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Create(task));
            Assert.Throws<ArgumentException>(() => processor.Create(new WTTask { Name = "Test" }));
            Assert.Throws<ArgumentException>(() => processor.Create(new WTTask { Name = "Test", WTUserId = Guid.Empty }));
        }
        [Test]
        public void CanRead()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);
            var user = db.Users.First();
            var ienum = processor.ReadAsIEnumerable(user.Id);
            var iquer = processor.ReadAsIQueryable(user.Id);
            Assert.AreEqual("First", ienum.First().Name);
            Assert.AreEqual("First", iquer.First().Name);
        }
        [Test]
        public void CanUpdate()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            task.Name = "Updated";
            var processor = new TaskProcessor(db);
            processor.Update(task);
            db.SaveChanges();
        }
        [Test]
        public void UpdateInvalid()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);

            var task = new WTTask();
            Assert.Throws<ArgumentException>(() => processor.Update(task));

            task = db.Tasks.First();
            task.Name = string.Empty;
            Assert.Throws<ArgumentException>(() => processor.Update(task));

            task = db.Tasks.First();
            task.Id = 0;
            Assert.Throws<ArgumentException>(() => processor.Update(task));

            task = db.Tasks.First();
            task.WTUserId = Guid.Empty;
            Assert.Throws<ArgumentException>(() => processor.Update(task));
        }
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            var processor = new TaskProcessor(db);
            processor.Delete(task.Id);
            db.SaveChanges();
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Delete(0));
        }
    }
}
