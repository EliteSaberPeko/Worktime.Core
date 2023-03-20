using NUnit.Framework;
using Worktime.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Worktime.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace Worktime.Tests.DatabaseTests
{
    internal static class Database
    {
        public static ApplicationContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationContext>().UseInMemoryDatabase("InMemory").Options;
            return new ApplicationContext(options);
        }
    }
}
