using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Cache;

namespace MoreDapperTests
{
    [TestClass]
    public class StringCacheTests
    {
        private class Foo
        {

        }

        [TestMethod]
        public void AddCommand()
        {
            var type = typeof(Foo);

            StringCache.Add(type, Operation.Insert, "insert 1");
            StringCache.Add(type, Operation.Insert, "insert 2");

            string cachedCommand;
            var value = StringCache.TryGetCommand(type, Operation.Insert, out cachedCommand);

            Assert.AreEqual(true, value);
            Assert.AreEqual("insert 1", cachedCommand);
        }
    }
}
