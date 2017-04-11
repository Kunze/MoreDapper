using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.CommandGenerator;
using UnitTestProject1;

namespace MoreDapperTests
{
    [TestClass]
    public class UpdateGeneratorTest
    {
        private class Foo
        {
            public int Id { get; set; }
            public string Foo1 { get; set; }
            public string Foo2 { get; set; }
        }

        [TestMethod]
        public void Update()
        {
            var generator = new UpdateGenerator();
            var command = generator.Generate(new Foo
            {
                Id = 5,
                Foo1 = "foo1",
                Foo2 = "foo2"
            });

            Assert.AreEqual("UPDATE Foo SET Foo1 = @Foo1, Foo2 = @Foo2 WHERE Id = @Id;", command);
        }
    }
}
