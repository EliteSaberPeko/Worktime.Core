using NUnit.Framework;
using System;
using System.Linq;
using Worktime.Core;
using Worktime.Core.CRUD;
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
            var processor = new UserProcessor(db);
            processor.Create(user);
            db.SaveChanges();
        }
        [Test]
        public void CreateEmpty()
        {
            var user = new WTUser();
            var db = Database.GetMemoryContext();
            var processor = new UserProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Create(user));
            Assert.Throws<ArgumentException>(() => processor.Create(new WTUser { Id = Guid.Empty }));
        }
        [Test]
        public void CanRead()
        {
            var db = Database.GetMemoryContext();
            var user = db.Users.First();
            var processor = new UserProcessor(db);
            Assert.AreEqual(user, processor.Read(user.Id));
        }
        [Test]
        public void CanDelete()
        {
            var db = Database.GetMemoryContext();
            var user = new WTUser { Id = Guid.NewGuid() };
            var processor = new UserProcessor(db);
            processor.Create(user);
            processor.Delete(user.Id);
            user = db.Users.First();
            processor.Delete(user.Id);
            db.SaveChanges();
        }
        [Test]
        public void DeleteEmpty()
        {
            var db = Database.GetMemoryContext();
            var processor = new UserProcessor(db);
            Assert.Throws<ArgumentException>(() => processor.Delete(Guid.NewGuid()));
            Assert.Throws<ArgumentException>(() => processor.Delete(Guid.Empty));
        }
    }
}
