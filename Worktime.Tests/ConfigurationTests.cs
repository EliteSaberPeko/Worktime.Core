using NUnit.Framework;
using Worktime.Core;

namespace Worktime.Tests
{
    public class ConfigurationTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void GetConnectionString()
        {
            var success = Configuration.GetConnectionString(out string connectionString, out string error);
            Assert.IsTrue(success);
        }

        
    }
}