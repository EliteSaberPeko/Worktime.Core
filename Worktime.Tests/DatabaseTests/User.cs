using NUnit.Framework;
using System;
using System.Linq;
using Worktime.Core;
using Worktime.Core.Models;

namespace Worktime.Tests.DatabaseTests
{
    internal class User
    {
        [SetUp]
        public void InitDb()
        {
            var user = new WTUser { Id = Guid.NewGuid() };
            var db = Database.GetMemoryContext();
            db.Database.EnsureDeleted();
            db.Users.Add(user);
            db.SaveChanges();
        }
        [Test]
        public void CanCreate()
        {
            var user = new WTUser { Id = Guid.NewGuid() };
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var result = processor.Create(user);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void CreateIvalid()
        {
            var user = new WTUser();
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);

            var result = processor.Create(user);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User is empty!", result.Message);

            result = processor.Create(new WTUser { Id = Guid.Empty });
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User is empty!", result.Message);

            user = db.Users.First();
            result = processor.Create(user);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User is exist!", result.Message);
        }
        [Test]
        public void CanRead()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            var processor = new Startup(db);
            Assert.AreEqual(user, processor.Read(user.Id));
            Assert.IsNull(processor.Read(Guid.NewGuid()));
        }
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var user = new WTUser { Id = Guid.NewGuid() };
            var processor = new Startup(db);
            processor.Create(user);

            var result = processor.Delete(user.Id);
            Assert.IsTrue(result.Success);

            user = db.Users.First();
            result = processor.Delete(user.Id);
            Assert.IsTrue(result.Success);
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new Startup(db);
            var result = processor.Delete(Guid.NewGuid());
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found!", result.Message);

            result = processor.Delete(Guid.Empty);
            Assert.IsFalse(result.Success);
            Assert.AreEqual("User was not found!", result.Message);
        }
    }
}
