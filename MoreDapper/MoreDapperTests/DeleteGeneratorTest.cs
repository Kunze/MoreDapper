using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MoreDapper.CommandGenerator;
using UnitTestProject1;
using System.Collections.Generic;
using System.Collections;

namespace MoreDapperTests
{
    [TestClass]
    public class DeleteGeneratorTest
    {
        private class Foo
        {
            public int Id { get; set; }
            public string Foo1 { get; set; }
            public string Foo2 { get; set; }
        }

        [TestMethod]
        public void Delete()
        {
            var generator = new DeleteGenerator();
            var command = generator.Generate(new Foo
            {
                Id = 5,
                Foo1 = "foo1",
                Foo2 = "foo2"
            });

            Assert.AreEqual("DELETE FROM Foo WHERE Id = @Id;", command);
        }
    }
}
