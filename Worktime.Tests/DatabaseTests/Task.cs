using NUnit.Framework;
using System;
using System.Collections.Generic;
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
            task = new WTTask()
            {
                Name = "First 2",
                Description = "Desc 2",
                Completed = false,
                TotalTime = 102,
                WTUserId = user.Id,
                User = user
            };
            db.Tasks.Add(task);
            db.SaveChanges();
        }

        #region Create
        [Test]
        public void CanCreate()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            var task = new WTTask()
            {
                Name = "First 3",
                Description = "Desc 3",
                Completed = false,
                TotalTime = 110,
                WTUserId = user.Id,
                User = user
            };
            var processor = new TaskProcessor(db);
            var result = processor.Create(task);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanCreateMany()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            List<WTTask> tasks = new List<WTTask>();
            for (int i = 0; i < 10; i++)
            {
                var task = new WTTask()
                {
                    Name = $"Test {i}",
                    Description = $"Desc {i}",
                    Completed = false,
                    TotalTime = 10 + i,
                    WTUserId = user.Id,
                    User = user
                };
                tasks.Add(task);
            }
            var processor = new TaskProcessor(db);
            var result = processor.Create(tasks);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CreateInvalid()
        {
            var task = new WTTask();
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);

            var result = processor.Create(task);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Name is empty!", result.Message);

            result = processor.Create(new WTTask { Name = "Test" });
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found!", result.Message);

            result = processor.Create(new WTTask { Name = "Test", WTUserId = Guid.Empty });
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found!", result.Message);
        }
        [Test]
        public void CreateInvalidMany()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            var processor = new TaskProcessor(db);

            List<WTTask> tasks = new List<WTTask>();
            for (int i = 0; i < 4; i++)
            {
                var task = new WTTask()
                {
                    Name = $"Test {i}",
                    Description = $"Desc {i}",
                    Completed = false,
                    TotalTime = 10 + i,
                    WTUserId = user.Id,
                    User = user
                };
                tasks.Add(task);
            }
            tasks[3].Name = string.Empty;
            var result = processor.Create(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Name is empty! Index: 3", result.Message);

            tasks[3].Name = "Test 777";
            tasks[2].WTUserId = Guid.Empty;
            result = processor.Create(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found! Index: 2", result.Message);
        }
        #endregion

        #region Read
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
        #endregion

        #region Update
        [Test]
        public void CanUpdate()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            task.Name = "Updated";
            var processor = new TaskProcessor(db);
            var result = processor.Update(task);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanUpdateMany()
        {
            var db = Database.GetMemoryContext();
            var tasks = db.Tasks.ToList();
            tasks[0].Name = "Updated";
            tasks[1].Description = "Description updated";
            var processor = new TaskProcessor(db);
            var result = processor.Update(tasks);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void UpdateInvalid()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);

            var task = new WTTask();
            var result = processor.Update(task);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);

            task = db.Tasks.First();
            task.WTUserId = Guid.Empty;
            result = processor.Update(task);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found!", result.Message);

            task.Name = string.Empty;
            result = processor.Update(task);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Name is empty!", result.Message);

            task.Id = 0;
            result = processor.Update(task);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);
        }
        [Test]
        public void UpdateInvalidMany()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);

            var tasks = db.Tasks.ToList();
            tasks[1].Id = 0;
            var result = processor.Update(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 1", result.Message);

            tasks[1].Id = db.Tasks.First().Id;
            tasks[0].WTUserId = Guid.Empty;
            result = processor.Update(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found! Index: 0", result.Message);

            tasks[0].WTUserId = db.Users.First().Id;
            tasks[0].Name = string.Empty;
            result = processor.Update(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Name is empty! Index: 0", result.Message);

            tasks[0].Name = "Test";
            tasks[1] = new WTTask();
            result = processor.Update(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 1", result.Message);
        }
        #endregion

        #region Delete
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var task = db.Tasks.First();
            var processor = new TaskProcessor(db);
            var result = processor.Delete(task);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CanDeleteMany()
        {
            var db = Database.GetMemoryContext();
            var tasks = db.Tasks.ToList();
            var processor = new TaskProcessor(db);
            var result = processor.Delete(tasks);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);
            var result = processor.Delete(new WTTask());
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found!", result.Message);
        }
        [Test]
        public void DeleteEmptyMany()
        {
            var db = Database.GetMemoryContext();
            var processor = new TaskProcessor(db);
            var tasks = db.Tasks.ToList();
            tasks[1] = new WTTask();
            var result = processor.Delete(tasks);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("Task was not found! Index: 1", result.Message);
        }
        #endregion
    }
}
