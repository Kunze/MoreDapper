using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.Scanner;

namespace UnitTestProject1
{
    [TestClass]
    public class SqlParameterScannerTest
    {
        [TestMethod]
        public void Scan()
        {
            var values = "@Foo, @Foo1, @Foo2, 'string', '@FooX', @Foo3, 100, '@Foo', 100.10";
            var parameters = SqlParameterScanner.Scan(values);

            Assert.AreEqual(4, parameters.Count);
            Assert.AreEqual("@Foo", parameters[0].Name);
            Assert.AreEqual("@Foo1", parameters[1].Name);
            Assert.AreEqual("@Foo2", parameters[2].Name);
            Assert.AreEqual("@Foo3", parameters[3].Name);
        }
    }
}
