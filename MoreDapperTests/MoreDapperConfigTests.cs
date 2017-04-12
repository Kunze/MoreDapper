using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Config;

namespace MoreDapperTests
{
    public class Foo
    {
        public string Foo1 { get; set; }
        public string Foo2 { get; set; }
    }

    [TestClass]
    public class MoreDapperConfigTests
    {
        [TestMethod]
        public void AddPrimaryKeyWithExpression()
        {
            MoreDapperConfig.AddPrimaryKey<Foo, string>((foo) => foo.Foo1);
            MoreDapperConfig.AddPrimaryKey<Foo, string>((foo) => foo.Foo2);

            var properties = MoreDapperConfig.GetKeysFor(typeof(Foo));
            Assert.AreEqual(3, properties.Count);
        }

        [TestMethod]
        public void AddPrimaryKeyWithTypeAndString()
        {
            var type = typeof(Foo);

            MoreDapperConfig.AddPrimaryKey(type, "Foo1");
            MoreDapperConfig.AddPrimaryKey(type, "Foo2");

            var properties = MoreDapperConfig.GetKeysFor(typeof(Foo));
            Assert.AreEqual(3, properties.Count);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void InvalidProperty()
        {
            var type = typeof(Foo);

            MoreDapperConfig.AddPrimaryKey(type, "FooX");
        }
    }
}
